using Primavera.Bot.Entities;
using Primavera.Bot.Entities.Results;
using Primavera.Hurakan.BotHandlers;
using Primavera.Hurakan.Core;
using Primavera.Hurakan.Handlers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Primavera.Bot.DevelopersNetworkTopic.Handlers
{
    [Export(typeof(IHandler))]
    [Export(typeof(TaskExecutionExampleHandler))]
    public class TaskExecutionExampleHandler : TopicHandlerBase
    {
        public override IMessage ProcessMessage(IMessage message)
        {
            Dictionary<string, List<BotMessage>> botMessages = new Dictionary<string, List<BotMessage>>();

            this.TopicId = "DevelopersNetworkTopic";
            this.TaskId = "TaskExecutionExample";

            this.Initialize(message as IntegrationMessage);

            // Verify if instances have been passed by previous handler
            if (this.Instances.Count == 0)
            {
                string err = "No instances to process.";
                this.ExecutionLog.AddTrace(Enums.TaskResultTraceType.Warning, err);
                return new BreakMessage(err);
            }

            foreach (Instance instance in this.Instances)
            {
                List<BotMessage> instanceMessages = new List<BotMessage>();

                foreach (Enterprise enterprise in instance.Enterprises)
                {
                    // Execute queries
                    this.BuildConnectionString(instance.ServerSql, instance.LoginSql, instance.PasswordSql, "PRI" + enterprise.Code);

                    /*  Example - Changing Data in The company Database
                    string strSQL = "UPDATE Clientes SET Nome = @NomeCliente WHERE Cliente = @Cliente";
                    this.ExecuteNonQuery(strSQL, new List<SqlParameter>()
                    {
                        new SqlParameter("NomeCliente", "NOME DO CLIENTE"),
                        new SqlParameter("Cliente", "ALCAD"),
                    });
                    */

                    // Execute simple query with result set
                    DataTable dt = this.Fill("SELECT SUM(TotalDeb) as total from clientes");
                    DataTable dtCustomersWithDebit = null;

                    DataTable dtResumoVendas = this.Fill(
                         "EXEC GCP_LST_RESUMO_VENDAS " +
                         "  @Tabela = '##ResumoVendasTesteBot', " +
                         "  @Campos = 'Mês = RIGHT (''00'' + LTRIM(STR(CAST(Mes AS nvarchar(30)))), 2), Vendas = CAST(LiquidoAnoAct AS INT)', " +
                         "  @MoedaVisualizacao = 'EUR', " +
                         "  @MoedaBase = 1, " +
                         "  @MoedaStocks = 'EUR',  " +
                         "  @AnoReferencia = '12/31/" + DateTime.Now.Year + "', " +
                         "  @AnoComparacao = '1/1/" + DateTime.Now.Year + "'");

                    double totDebito = (double)dt.Rows[0]["total"];

                    if (totDebito > 0)
                    {
                        dtCustomersWithDebit = this.Fill("SELECT Cliente, TotalDeb FROM Clientes WHERE TotalDeb > 0 Order By TotalDeb DESC");
                    }

                    /*  Example - Use of ERP Engine
                    using (ErpHelper erpHelper = new ErpHelper())
                    {
                        string customerId = "ALCAD";
                        erpHelper.SetErpConnectionString(instance, enterprise.Code);
                        dynamic customerObject = erpHelper.Erp.Base.Clientes.Edita(customerId);
                        customerObject.Nome = "Nome";
                    }
                    */

                    foreach (User user in instance.Users)
                    {
                        // Create simple message
                        //BotMessage companyMessage = this.InitializeNewMessage(instance, enterprise, EmptyUserCodePlaceHolder, "A tarefa 'TaskExecutionExample' foi concluída com sucesso.");
                        //companyMessage.CompanyId = enterprise.Code;
                        //instanceMessages.Add(companyMessage);

                        // Create larger message With Table
                        //BotMessage bigMessage = this.InitializeNewMessage(instance, enterprise, EmptyUserCodePlaceHolder, "Esta mensagem contém {0} e {1} e {2}");
                        //bigMessage.CompanyId = enterprise.Code;
                        //bigMessage.Actions = this.AddActionsTable();
                        //bigMessage.Results = this.AddResultsTable(dt);
                        //instanceMessages.Add(bigMessage);

                        // Create Debit message
                        BotMessage debitMessage = this.InitializeNewMessage(instance, enterprise, EmptyUserCodePlaceHolder, "Olá. Existem neste momento " + totDebito.ToString() + " EUR de créditos pendentes de clientes. Saiba mais {0}.");
                        debitMessage.CompanyId = enterprise.Code;
                        debitMessage.Actions = this.AddDebitMessageActions();
                        debitMessage.Results = this.AddDebitMessageResults(dtCustomersWithDebit);
                        instanceMessages.Add(debitMessage);

                        // Create larger message With Graph
                        //BotMessage graphMessage = this.InitializeNewMessage(instance, enterprise, EmptyUserCodePlaceHolder, "Veja {0} as suas vendas (lines list)!");
                        //graphMessage.CompanyId = enterprise.Code;
                        //graphMessage.Actions = this.AddActionsGraph();
                        //graphMessage.Results = this.AddResultsGraph();
                        //instanceMessages.Add(graphMessage);

                        // Create larger message With Graph, using a datatable as input
                        BotMessage graphMessage2 = this.InitializeNewMessage(instance, enterprise, EmptyUserCodePlaceHolder, "Olá. Clica {0} para veres os valores das tuas vendas deste ano.");
                        graphMessage2.CompanyId = enterprise.Code;
                        graphMessage2.Actions = this.AddActionsGraph();
                        graphMessage2.Results = this.AddResultsGraphDatatable(dtResumoVendas);
                        instanceMessages.Add(graphMessage2);
                    }
                }

                // add the message to resultset.
                if (this.BotMessages.ContainsKey(instance.Id))
                {
                    this.BotMessages[instance.Id] = instanceMessages;
                }
                else
                {
                    this.BotMessages.Add(instance.Id, instanceMessages);
                }
            }

            return this.BuildIntegrationMessage(this.BotMessages);
        }

        private Collection<BotMessageAction> AddActionsTable()
        {
            return new Collection<BotMessageAction>()
            {
                new BotMessageAction()
                {
                    ActionIndex = 0,
                    ActionType = BotMessageActionType.Drilldown,
                    ActionParameters = "GCP|1|GCP_MOSTRAMANUTENCAO|Manutencao=mnuTabClientes|Entidade=ALCAD",
                    Text = "operações de Drill-Down"
                },
                new BotMessageAction()
                {
                    ActionIndex = 1,
                    ActionType = BotMessageActionType.ScheduleUserTask,
                    TaskParameters = new BotMessageActionTaskParameters()
                    {
                        TopicId = this.TopicId,
                        TaskId = "ExecuteEntityUpdate",
                        PipelineId = "ExecuteEntityUpdatePipeline",
                        ExecutionParameters = "Company=DEMO|Customers=ALCAD"
                    },
                    Text = "Agendamento de tarefas"
                },
                new BotMessageAction()
                {
                    ActionIndex = 2,
                    ActionType = BotMessageActionType.ResultsPage,
                    Text = "Tabela - Clientes"
                }
            };
        }

        private Collection<BotMessageAction> AddDebitMessageActions()
        {
            return new Collection<BotMessageAction>()
            {
                new BotMessageAction()
                {
                    ActionIndex = 0,
                    ActionType = BotMessageActionType.ResultsPage,
                    Text = "Aqui"
                }                
            };
        }

        private Collection<BotMessageAction> AddActionsGraph()
        {
            return new Collection<BotMessageAction>()
            {
                new BotMessageAction()
                {
                    ActionIndex = 3,
                    ActionType = BotMessageActionType.ResultsPage,
                    Text = "Aqui"
                }
            };
        }

        private BotMessageResults AddDebitMessageResults(DataTable dtCustomersWithDebit)
        {
            BotMessageResults results = new BotMessageResults();
            results.AddDataTableToResultSet(dtCustomersWithDebit);

            results.ViewConfig.Add(new Entities.Results.TableView()
            {
                Order = 0,
                Columns = new System.Collections.ObjectModel.Collection<Entities.Results.Column>()
                    {
                        new Entities.Results.Column()
                        {
                            Name = "Cliente",
                            Visible = true
                        },
                        new Entities.Results.Column()
                        {
                            Name = "Crédito total",
                            Visible = true
                        },
                    },
                ResultSet = 0,
                ShowTitle = true,
                Title = "Clientes com créditos pendentes."
            });


            foreach (List<Cell> linha in results.ResultSets.First())
            {
                // Add action for the first column (entity, ex: "Sofrio")
                linha[0].Action = new BotMessageAction()
                {
                    ActionIndex = 0,
                    ActionType = BotMessageActionType.Drilldown,
                    ActionParameters = string.Concat("GCP|1|GCP_MOSTRAMANUTENCAO|Manutencao=mnuTabClientes|Entidade=", linha[0].Value),
                    Text = linha[0].Value
                };

                linha[1].Action = new BotMessageAction()
                {
                    ActionIndex = 1,
                    ActionType = BotMessageActionType.Drilldown,
                    ActionParameters = "GCP|1|GCP_MOSTRAEXPLORACAO|Exploracao=mnuExpCCPendentesGrelha|TipoEntidade=C|Entidade=" + linha[0].Value,
                    Text = linha[1].Value
                };
            }

            return results;
        }

        private BotMessageResults AddResultsTable(DataTable dtClientes)
        {
            BotMessageResults results = new BotMessageResults();
            results.AddDataTableToResultSet(dtClientes);

            results.ViewConfig.Add(new Entities.Results.TableView()
            {
                Order = 0,
                Columns = new System.Collections.ObjectModel.Collection<Entities.Results.Column>()
                    {
                        new Entities.Results.Column()
                        {
                            Name = "Cliente",
                            Visible = true
                        },
                        new Entities.Results.Column()
                        {
                            Name = "Nome",
                            Visible = true
                        },
                    },
                ResultSet = 0,
                ShowTitle = true,
                Title = "Clientes"
            });

            return results;
        }

        private BotMessageResults AddResultsGraphDatatable(DataTable dtResumoVendas)
        {
            BotMessageResults results = new BotMessageResults();

            results.AddDataTableToResultSet(dtResumoVendas);

            results.ViewConfig.Add(new Entities.Results.GraphView()
            {
                Columns = new Collection<Column>()
                {
                    new Column() { Name = "Mês", Visible = true, Axis = "x" },
                    new Column() { Name = "Vendas", Visible = true }
                },

                Order = 0,
                GraphType = GraphType.Bar,
                ResultSet = 0,
                ShowTitle = true,
                Title = "Vendas"
            });

            return results;

        }

        private BotMessageResults AddResultsGraph()
        {
            BotMessageResults results = new BotMessageResults();

            Collection<Line> linesList = new Collection<Line>()
            {
                new Line()
                {
                    Cells = new Collection<Cell>()
                    {
                        new Cell() { Action = null, Value = "Janeiro"},
                        new Cell() { Action = null, Value = "32" },
                    }
                },
                new Line()
                {
                    Cells = new Collection<Cell>()
                    {
                        new Cell() { Action = null, Value = "Fevereiro" },
                        new Cell() { Action = null, Value = "45" },
                    }
                },
                new Line()
                {
                    Cells = new Collection<Cell>()
                    {
                        new Cell() { Action = null, Value = "Março" },
                        new Cell() { Action = null, Value = "54" },
                    }
                },
                new Line()
                {
                    Cells = new Collection<Cell>()
                    {
                        new Cell() { Action = null, Value = "Abril" },
                        new Cell() { Action = null, Value = "110" },
                    }
                },
                new Line()
                {
                    Cells = new Collection<Cell>()
                    {
                        new Cell() { Action = null, Value = "Maio" },
                        new Cell() { Action = null, Value = "150" },
                    }
                },
                new Line()
                {
                    Cells = new Collection<Cell>()
                    {
                        new Cell() { Action = null, Value = "Junho" },
                        new Cell() { Action = null, Value = "120" },
                    }
                },
                    new Line()
                {
                    Cells = new Collection<Cell>()
                    {
                        new Cell() { Action = null, Value = "Julho" },
                        new Cell() { Action = null, Value = "80" },
                    }
                },
                    new Line()
                {
                    Cells = new Collection<Cell>()
                    {
                        new Cell() { Action = null, Value = "Agosto" },
                        new Cell() { Action = null, Value = "10" },
                    }
                },
                    new Line()
                {
                    Cells = new Collection<Cell>()
                    {
                        new Cell() { Action = null, Value = "Setembro" },
                        new Cell() { Action = null, Value = "5" },
                    }
                },
                    new Line()
                {
                    Cells = new Collection<Cell>()
                    {
                        new Cell() { Action = null, Value = "Outubro" },
                        new Cell() { Action = null, Value = "15" },
                    }
                },
                    new Line()
                {
                    Cells = new Collection<Cell>()
                    {
                        new Cell() { Action = null, Value = "Novembro" },
                        new Cell() { Action = null, Value = "25" },
                    }
                },
                    new Line()
                {
                    Cells = new Collection<Cell>()
                    {
                        new Cell() { Action = null, Value = "Dezembro" },
                        new Cell() { Action = null, Value = "70" },
                    }
                }
            };

            results.AddLinesListToResultSet(linesList);

            results.ViewConfig.Add(new Entities.Results.GraphView()
            {
                Columns = new Collection<Column>()
                {
                    new Column() { Name = "Mês", Visible = true, Axis = "x" },
                    new Column() { Name = "Vendas", Visible = true }
                },

                Order = 0,
                GraphType = GraphType.Bar,
                ResultSet = 0,
                ShowTitle = true,
                Title = "Vendas"
            });

            return results;

        }

    }
}
