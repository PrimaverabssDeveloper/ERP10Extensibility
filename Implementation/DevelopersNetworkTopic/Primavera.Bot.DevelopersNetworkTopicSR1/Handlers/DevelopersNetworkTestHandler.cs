using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Data;
using Primavera.Bot.Entities;
using Primavera.Bot.Entities.Results;
using Primavera.Bot.Managers.Interfaces;
using Primavera.Hurakan.BotHandlers;
using Primavera.Hurakan.Core;
using Primavera.Hurakan.Handlers;

namespace Primavera.Bot.DevelopersNetworkTopic
{
    /// <summary>
    /// This is the class that implements the topic handler to produce template messages.
    /// </summary>
    /// <seealso cref="Primavera.Hurakan.BotHandlers.TopicHandlerBase" />
    [Export(typeof(IHandler))]
    [Export(typeof(DevelopersNetworkTestHandler))]
    public class DevelopersNetworkTestHandler : TopicHandlerBase
    {
        #region Private Members

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DevelopersNetworkTestHandler"/> class.
        /// </summary>
        public DevelopersNetworkTestHandler()
            : base()
        {
        }

        #endregion

        #region Base Overrides

        /// <summary>
        /// Processes the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>The processed message.</returns>
        public override IMessage ProcessMessage(IMessage message)
        {
            // This very simple example generates a message for ALL Instances/Enterprises/Users with sample text.

            try
            {
                // Initialize objects using base
                this.Initialize((IntegrationMessage)message);

                // Verify if instances have been passed by previous handler
                if (this.Instances.Count == 0)
                {
                    string err = "No instances to process.";
                    this.ExecutionLog.AddTrace(Enums.TaskResultTraceType.Warning, err);
                    return new BreakMessage(err);
                }

                // Iterate through instances and create messages.
                foreach (Instance instance in this.Instances)
                {
                    this.TenantId = instance.TenantId;
                    this.Scope = instance.Id;
                    this.OrganizationId = instance.OrganizationId;

                    // Create a list to add BOT messages
                    List<BotMessage> instanceMessages = new List<BotMessage>();

                    // Implement Topic Handler specific behavior here

                    // Pattern to create messages the message objects
                    using (var manager = this.CreateManager<IBotMessageManager, BotMessage>(this.Instances))
                    {
                        foreach (Enterprise enterprise in instance.Enterprises)
                        {
                            foreach (User user in instance.Users)
                            {
                                this.AppContext.UserName = user.Code;
                                manager.SetAppContext(this.AppContext);

                                // Get a BotMessage object with default data.
                                // "EmptyUserCodePlaceHolder" is managed by the user bot messages handler
                                BotMessage createdMessage = this.InitializeNewMessage(instance, enterprise, EmptyUserCodePlaceHolder, "Hello World from Bot!");

                                // Customize required message properties
                                createdMessage.ActiveContexts = "Clientes$frmTabClientes";

                                // Add actions
                                var actions = this.AddActions(createdMessage);

                                // Add results - example for a datatable
                                DataTable results = new DataTable();
                                // add columns and data
                                // add it to the message
                                this.AddResults(createdMessage, results);

                                instanceMessages.Add(createdMessage);
                            }
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

                // Return for save handler.
                return this.BuildIntegrationMessage(this.BotMessages);
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }

                this.Log(LogEvent.Error(ex));

                throw;
            }
        }

        #endregion

        /// <summary>
        /// Adds the actions.
        /// </summary>
        /// <param name="companyId">The company identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="enterpriseCustomer">The enterprise customer.</param>
        /// <returns>
        /// The actions for this message.
        /// </returns>
        private Collection<BotMessageAction> AddActions(BotMessage b)
        {
            return new Collection<BotMessageAction>()
            {
                // Add Actions - example pkb
                new BotMessageAction()
                {
                    ActionIndex = 0,
                    ActionType = BotMessageActionType.PkbArticle,
                    ActionParameters = "Sample action Parameters. in this case, a PKB article id",
                    Text = "Sample text that should appear on the link"
                }

                // Add more actions as required  
            };
        }

        /// <summary>
        /// Adds the results.
        /// </summary>
        /// <param name="companyMessage">The company message.</param>
        /// <param name="resultsDiff">The results difference.</param>
        private void AddResults(BotMessage companyMessage, DataTable resultsDiff)
        {
            var results = new BotMessageResults();
            results.AddDataTableToResultSet(resultsDiff);

            // for each result we'll add the correspondent action
            foreach (List<Cell> line in results.ResultSets[0])
            {
                // Add action for the first column
                line[1].Action = new BotMessageAction()
                {
                    ActionIndex = 0,
                    ActionType = BotMessageActionType.Drilldown,
                    ActionParameters = "Parameters.",
                    Text = line[1].Value
                };
            }

            // Now we need to define how the table is displayed (view config)
            this.AddViewConfig(results);
            companyMessage.Results = results;
        }

        /// <summary>
        /// Adds the view configuration.
        /// </summary>
        /// <param name="results">The results.</param>
        private void AddViewConfig(BotMessageResults results)
        {
            results.ViewConfig.Add(
                new Entities.Results.TableView()
                {
                    Order = 0,
                    Columns = new System.Collections.ObjectModel.Collection<Entities.Results.Column>()
                    {
                        new Entities.Results.Column()
                        {
                            Name = string.Empty,
                            Visible = false
                        },
                        new Entities.Results.Column()
                        {
                            Name = Entities.Helpers.Functions.GetAllAvailableCulturesForResource(Properties.Resources.ResourceManager, "ResourceName", null),
                            Visible = true
                        },
                    },
                    ResultSet = 0,
                    ShowTitle = true,
                    Title = Entities.Helpers.Functions.GetAllAvailableCulturesForResource(Properties.Resources.ResourceManager, "ResourceName", null)
                });
        }

        #region IDisposable Members (overriden)

        /// <summary>
        /// Called whenever the object instance needs to clean up.
        /// Releases unmanaged and managed resources (optionally).
        /// <c>Dispose(bool disposing)</c> executes in two distinct scenarios:
        /// If disposing equals true, the method has been called directly or indirectly by a user's code.
        /// - Managed and unmanaged resources can be disposed.
        /// If disposing equals false, the method has been called by the runtime from inside the <c>finalizer</c> and you should not reference other objects.
        /// - Only unmanaged resources can be disposed.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called

            if (!this.Disposed)
            {
                // Dispose managed resources

                if (disposing)
                {
                }

                // Dispose unmanaged resources

                // insert your code here...
            }

            // Dispose on base class

            base.Dispose(disposing);
        }

        #endregion   
    }
}
