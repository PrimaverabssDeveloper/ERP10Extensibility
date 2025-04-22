using System;
using System.Windows.Forms;
using ErpBS100;
using primavera.extensibility.kits.Code;
using StdBE100;
using StdPlatBS100;

namespace primavera.extensibility.kits
{
    public partial class frmMain : Form
    {
        string empresa;
        string user;
        string password;

        ErpBS motor = new ErpBS();
        StdPlatBS plataforma = new StdPlatBS();

        StdBSConfApl objAplConf = new StdBSConfApl();

        private BaseBS _baseBs = null;
        private VendasBS _vendasBs = null;
        private InternalBS _internalBS = null;
        private ComprasBS _comprasBS = null;
        private InventarioBS _inventarioBS = null;

        StdPlatBS100.StdBSDialogoEspera objDialogo = null;

        StdBE100.StdBETipos.EnumTipoPlataforma objTipoPlataforma;
        public frmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.textBox5.Text = "";
            Cursor.Current = Cursors.WaitCursor;
            empresa = textBox1.Text;
            user = textBox2.Text;
            password = textBox3.Text;

            if (!this.checkBox1.Checked)
            {
                objTipoPlataforma = StdBE100.StdBETipos.EnumTipoPlataforma.tpEmpresarial;
            }
            else
            {
                objTipoPlataforma = StdBE100.StdBETipos.EnumTipoPlataforma.tpProfissional;
            }

            objAplConf.Instancia = this.textBox4.Text;
            objAplConf.AbvtApl = "ERP";
            objAplConf.PwdUtilizador = password;
            objAplConf.Utilizador = user;

            objAplConf.LicVersaoMinima = "10.00";

            StdBETransaccao objStdTransac = new StdBE100.StdBETransaccao();

            try
            {
                plataforma.AbrePlataformaEmpresa(empresa, null, objAplConf, objTipoPlataforma);
            }
            catch (Exception ex)
            {
                this.textBox5.AppendText(ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine);
            }

            if (plataforma.Inicializada)
            {
                this.textBox5.AppendText("Plataforma inicializada" + Environment.NewLine);
                this.textBox5.AppendText(plataforma.Contexto.Utilizador.Utilizador + Environment.NewLine);


                motor.Plataforma = plataforma;




                try
                {
                    motor.AbreEmpresaTrabalho(objTipoPlataforma, empresa, plataforma.Contexto.Utilizador.Utilizador, plataforma.Contexto.Utilizador.Password, strInstancia: this.textBox4.Text);


                    _vendasBs = new VendasBS(motor, plataforma);
                    _baseBs = new BaseBS(motor, plataforma);
                    _internalBS = new InternalBS(motor, plataforma);
                    _comprasBS = new ComprasBS(motor, plataforma);
                    _inventarioBS = new InventarioBS(motor, plataforma);

                    opVendas_CheckedChanged(null, EventArgs.Empty);
                    opEstornoVendas_CheckedChanged(null, EventArgs.Empty);
                    opConverterVendas_CheckedChanged(null, EventArgs.Empty);
                    opTransformarVendas_CheckedChanged(null, EventArgs.Empty);

                    groupBox1.Enabled = true;
                    groupBox2.Enabled = true;
                    groupBox3.Enabled = true;
                    groupBox4.Enabled = true;
                    groupBox5.Enabled = true;

                    cmbModuloDe.SelectedIndex = 0;
                    cmbModuloPara.SelectedIndex = 0;

                    this.textBox5.AppendText("Empresa aberta" + Environment.NewLine);

                }
                catch (Exception ex)
                {
                    this.textBox5.AppendText(ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine);
                }

            }
            Cursor.Current = Cursors.Default;
        }

        #region OPCOES
        private void opVendas_CheckedChanged(object sender, EventArgs e)
        {
            txtTipoDoc.Text = "FA";
            txtSerie.Text = motor.Base.Series.DaSerieDefeito(ConstantesPrimavera100.Modulos.Vendas, txtTipoDoc.Text);
        }

        private void opCompras_CheckedChanged(object sender, EventArgs e)
        {
            txtTipoDoc.Text = "VFA";
            txtSerie.Text = motor.Base.Series.DaSerieDefeito(ConstantesPrimavera100.Modulos.Compras, txtTipoDoc.Text);
        }

        private void opInternos_CheckedChanged(object sender, EventArgs e)
        {
            txtTipoDoc.Text = "ES";
            txtSerie.Text = motor.Base.Series.DaSerieDefeito(ConstantesPrimavera100.Modulos.Internos, txtTipoDoc.Text);
        }

        private void opTransferencias_CheckedChanged(object sender, EventArgs e)
        {
            txtTipoDoc.Text = "TRA";
            txtSerie.Text = motor.Base.Series.DaSerieDefeito(ConstantesPrimavera100.Modulos.Transferencias, txtTipoDoc.Text);
        }

        private void opComposicoes_CheckedChanged(object sender, EventArgs e)
        {
            txtTipoDoc.Text = "COM";
            txtSerie.Text = motor.Base.Series.DaSerieDefeito(ConstantesPrimavera100.Modulos.Composicoes, txtTipoDoc.Text);
        }

        private void opEstornoVendas_CheckedChanged(object sender, EventArgs e)
        {
            txtTipoDocEstorno.Text = "FA";
            txtSerieEstorno.Text = motor.Base.Series.DaSerieDefeito(ConstantesPrimavera100.Modulos.Vendas, txtTipoDocEstorno.Text);
        }

        private void opEstornoCompras_CheckedChanged(object sender, EventArgs e)
        {
            txtTipoDocEstorno.Text = "VFA";
            txtSerieEstorno.Text = motor.Base.Series.DaSerieDefeito(ConstantesPrimavera100.Modulos.Compras, txtTipoDocEstorno.Text);
        }

        private void opEstornoInternos_CheckedChanged(object sender, EventArgs e)
        {
            txtTipoDocEstorno.Text = "ES";
            txtSerieEstorno.Text = motor.Base.Series.DaSerieDefeito(ConstantesPrimavera100.Modulos.Internos, txtTipoDocEstorno.Text);
        }

        private void opConverterVendas_CheckedChanged(object sender, EventArgs e)
        {
            txtTipoDocOrigConv.Text = "ECL";
            txtSerieOrigConv.Text = motor.Base.Series.DaSerieDefeito(ConstantesPrimavera100.Modulos.Vendas, txtTipoDocOrigConv.Text);

            txtTipoDocDestConv.Text = "FA";
            txtSerieDestConv.Text = motor.Base.Series.DaSerieDefeito(ConstantesPrimavera100.Modulos.Vendas, txtTipoDocDestConv.Text);

        }

        private void opConverterCompras_CheckedChanged(object sender, EventArgs e)
        {
            txtTipoDocOrigConv.Text = "ECF";
            txtSerieOrigConv.Text = motor.Base.Series.DaSerieDefeito(ConstantesPrimavera100.Modulos.Compras, txtTipoDocOrigConv.Text);

            txtTipoDocDestConv.Text = "VFA";
            txtSerieDestConv.Text = motor.Base.Series.DaSerieDefeito(ConstantesPrimavera100.Modulos.Compras, txtTipoDocDestConv.Text);
        }

        private void opTransformarVendas_CheckedChanged(object sender, EventArgs e)
        {
            txtTipoDocOrig.Text = "ECL";
            txtSerieOrig.Text = motor.Base.Series.DaSerieDefeito(ConstantesPrimavera100.Modulos.Vendas, txtTipoDocOrig.Text);

            txtTipoDocDest.Text = "FA";
            txtSerieDest.Text = motor.Base.Series.DaSerieDefeito(ConstantesPrimavera100.Modulos.Vendas, txtTipoDocDest.Text);
        }

        private void opTransformarCompras_CheckedChanged(object sender, EventArgs e)
        {
            txtTipoDocOrig.Text = "ECF";
            txtSerieOrig.Text = motor.Base.Series.DaSerieDefeito(ConstantesPrimavera100.Modulos.Compras, txtTipoDocOrig.Text);

            txtTipoDocDest.Text = "VFA";
            txtSerieDest.Text = motor.Base.Series.DaSerieDefeito(ConstantesPrimavera100.Modulos.Compras, txtTipoDocDest.Text);
        }


        #endregion

        private string artigoKit = string.Empty;

        private void cmbModuloDe_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbModuloDe.SelectedIndex)
            {
                case 0:
                    txtTipoDocDe.Text = "FA";
                    txtSerieDe.Text = motor.Base.Series.DaSerieDefeito(ConstantesPrimavera100.Modulos.Vendas, txtTipoDocDe.Text);
                    break;
                case 1:
                    txtTipoDocDe.Text = "VFA";
                    txtSerieDe.Text = motor.Base.Series.DaSerieDefeito(ConstantesPrimavera100.Modulos.Compras, txtTipoDocDe.Text);
                    break;

                case 2:
                    txtTipoDocDe.Text = "ES";
                    txtSerieDe.Text = motor.Base.Series.DaSerieDefeito(ConstantesPrimavera100.Modulos.Internos, txtTipoDocDe.Text);
                    break;

                case 3:
                    txtTipoDocDe.Text = "TRA";
                    txtSerieDe.Text = motor.Base.Series.DaSerieDefeito(ConstantesPrimavera100.Modulos.Transferencias, txtTipoDocDe.Text);
                    break;


            }
        }

        private void cmbModuloPara_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbModuloPara.SelectedIndex)
            {
                case 0:
                    txtTipoDocPara.Text = "FA";
                    txtSeriePara.Text = motor.Base.Series.DaSerieDefeito(ConstantesPrimavera100.Modulos.Vendas, txtTipoDocPara.Text);
                    break;
                case 1:
                    txtTipoDocPara.Text = "VFA";
                    txtSeriePara.Text = motor.Base.Series.DaSerieDefeito(ConstantesPrimavera100.Modulos.Compras, txtTipoDocPara.Text);
                    break;

                case 2:
                    txtTipoDocPara.Text = "ES";
                    txtSeriePara.Text = motor.Base.Series.DaSerieDefeito(ConstantesPrimavera100.Modulos.Internos, txtTipoDocPara.Text);
                    break;

                case 3:
                    txtTipoDocPara.Text = "TRA";
                    txtSeriePara.Text = motor.Base.Series.DaSerieDefeito(ConstantesPrimavera100.Modulos.Transferencias, txtTipoDocPara.Text);
                    break;


            }
        }

        #region BUTTON_ACTION
        private void btGravar_Click(object sender, EventArgs e)
        {
            try
            {
                objDialogo = plataforma.MensagensDialogos.MostraDialogoEsperaEx("A Processar Documentos ...");


                CriarArtigoKitComStock();

                if (opVendas.Checked)
                {
                    this.textBox5.AppendText(_vendasBs.KIT_Vendas_Documento(txtTipoDoc.Text, txtSerie.Text, artigoKit) + Environment.NewLine);
                }

                if (opCompras.Checked)
                {
                    this.textBox5.AppendText(_comprasBS.KIT_Compras_Documento(txtTipoDoc.Text, txtSerie.Text, artigoKit) + Environment.NewLine);
                }

                if (opInternos.Checked)
                {
                    this.textBox5.AppendText(_internalBS.KIT_Grava_Interno(txtTipoDoc.Text, txtSerie.Text, artigoKit) + Environment.NewLine);
                }

                if (opTransferencias.Checked)
                {
                    //Entrada de Stock Artigo Tapete
                    this.textBox5.AppendText(_internalBS.KIT_Grava_Interno("ES", "A", "TAPETE", 20) + Environment.NewLine);

                    this.textBox5.AppendText(_inventarioBS.KIT_Grava_Trasnferencia(txtTipoDoc.Text, txtSerie.Text, artigoKit) + Environment.NewLine);
                }

                if (opComposicoes.Checked)
                {
                    var composto = _baseBs.CriaArtigo("CMP", "Composto");
                    composto.Classe = 2;//Artigo composto
                    motor.Base.Artigos.Actualiza(composto);

                    this.textBox5.AppendText(_inventarioBS.KIT_Grava_Composicao(txtTipoDoc.Text, txtSerie.Text, composto.Artigo, artigoKit) + Environment.NewLine);
                }

            }
            catch (Exception ex)
            {
                plataforma.MensagensDialogos.MostraErro(ex.Message);
            }
            finally
            {
                //Termina o dialogo
                objDialogo.Termina();
            }
        }

        private void btConverter_Click(object sender, EventArgs e)
        {
            try
            {
                objDialogo = plataforma.MensagensDialogos.MostraDialogoEsperaEx("A Processar Documentos ...");


                CriarArtigoKitComStock();

                if (opConverterVendas.Checked)
                {
                    this.textBox5.AppendText(_vendasBs.KIT_Vendas_ConverteDocs(txtTipoDocOrigConv.Text, txtSerieOrigConv.Text, txtTipoDocDestConv.Text, txtSerieDestConv.Text, artigoKit) + Environment.NewLine);
                }

                if (opConverterCompras.Checked)
                {
                    this.textBox5.AppendText(_comprasBS.KIT_Compras_ConverteDocs(txtTipoDocOrigConv.Text, txtSerieOrigConv.Text, txtTipoDocDestConv.Text, txtSerieDestConv.Text, artigoKit) + Environment.NewLine);
                }


            }
            catch (Exception ex)
            {
                plataforma.MensagensDialogos.MostraErro(ex.Message);
            }
            finally
            {
                //Termina o dialogo
                objDialogo.Termina();
            }

        }

        private void btCopiar_Click(object sender, EventArgs e)
        {
            try
            {
                objDialogo = plataforma.MensagensDialogos.MostraDialogoEsperaEx("A Processar Documentos ...");

                CriarArtigoKitComStock();

                this.textBox5.AppendText(_internalBS.KIT_CopiaLinhas(
                    artigoKit,
                    daModulo(cmbModuloDe.Text),
                    txtTipoDocDe.Text,
                    txtSerieDe.Text,
                    daModulo(cmbModuloPara.Text),
                    txtTipoDocPara.Text,
                    txtSeriePara.Text) + Environment.NewLine);
            }
            catch (Exception ex)
            {
                plataforma.MensagensDialogos.MostraErro(ex.Message);
            }
            finally
            {
                //Termina o dialogo
                objDialogo.Termina();
            }
        }

        private void btEstorno_Click(object sender, EventArgs e)
        {
            try
            {
                objDialogo = plataforma.MensagensDialogos.MostraDialogoEsperaEx("A Processar Documentos ...");

                CriarArtigoKitComStock();

                if (opEstornoVendas.Checked)
                {
                    this.textBox5.AppendText(_vendasBs.KIT_Vendas_EstornaDocumento(txtTipoDocEstorno.Text, txtSerieEstorno.Text, artigoKit) + Environment.NewLine);
                }

                if (opEstornoCompras.Checked)
                {
                    this.textBox5.AppendText(_comprasBS.KIT_Compras_EstornaDocumento(txtTipoDocEstorno.Text, txtSerieEstorno.Text, artigoKit) + Environment.NewLine);
                }

                if (opEstornoInternos.Checked)
                {
                    this.textBox5.AppendText(_internalBS.KIT_Internos_EstornaDocumento(txtTipoDocEstorno.Text, txtSerieEstorno.Text, artigoKit) + Environment.NewLine);
                }


            }
            catch (Exception ex)
            {
                plataforma.MensagensDialogos.MostraErro(ex.Message);
            }
            finally
            {
                //Termina o dialogo
                objDialogo.Termina();
            }
        }

        private void btTransformar_Click(object sender, EventArgs e)
        {
            try
            {
                objDialogo = plataforma.MensagensDialogos.MostraDialogoEsperaEx("A Processar Documentos ...");
                
                CriarArtigoKitComStock();

                if (opTransformarVendas.Checked)
                {
                    this.textBox5.AppendText(_vendasBs.KIT_Vendas_Transforma(txtTipoDocOrig.Text, txtSerieOrig.Text, txtTipoDocDest.Text, txtSerieDest.Text, artigoKit) + Environment.NewLine);
                }

                if (opTransformarCompras.Checked)
                {
                    this.textBox5.AppendText(_comprasBS.KIT_Compras_Transforma(txtTipoDocOrig.Text, txtSerieOrig.Text, txtTipoDocDest.Text, txtSerieDest.Text, artigoKit) + Environment.NewLine);
                }

            }
            catch (Exception ex)
            {
                plataforma.MensagensDialogos.MostraErro(ex.Message);
            }
            finally
            {
                //Termina o dialogo
                objDialogo.Termina();
            }
        }

        #endregion
        private string daModulo(string modulo)
        {
            switch (modulo)
            {
                case "Interno": return ConstantesPrimavera100.Modulos.Internos;
                case "Venda": return ConstantesPrimavera100.Modulos.Vendas;
                case "Compra": return ConstantesPrimavera100.Modulos.Compras;
                case "Trasferencia": return ConstantesPrimavera100.Modulos.Transferencias;
            }

            return string.Empty;
        }

        private void CriarArtigoKitComStock()
        {
            if (string.IsNullOrEmpty(artigoKit))
            {
                var artigo = _baseBs.CriaArtigosKitTeste();

                artigoKit = artigo.Artigo;

                this.textBox5.AppendText($"Artigo KIT {artigoKit} {Environment.NewLine}");

                this.textBox5.AppendText(_internalBS.KIT_Grava_Interno("ES", "A", artigo.Artigo, 100) + Environment.NewLine);
            }
        }
        
    }
}