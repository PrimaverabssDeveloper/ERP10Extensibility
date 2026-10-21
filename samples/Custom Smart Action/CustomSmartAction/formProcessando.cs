using System;
using System.Windows.Forms;

namespace CustomSmartAction
{
    /// <summary>
    /// Loader nao-modal que permanece visivel durante o processamento da Smart Action
    /// (entre o retorno de BeforeExecution e a chamada de AfterExecution).
    /// Fecha-se de tres formas:
    ///   1. CloseForm() chamado pelo AfterExecution quando o AI termina.
    ///   2. Botao "Cancelar" clicado pelo utilizador (caso suspeite de erro).
    ///   3. Timeout automatico - seguranca para quando o AfterExecution nunca chega.
    /// </summary>
    public partial class formProcessando : Form
    {
        // Timeout por defeito: 3 minutos. Ajustar conforme o tempo esperado do AI.
        private const int TimeoutMinutes = 3;

        private bool _allowClose;

        public formProcessando()
        {
            InitializeComponent();

            timerTimeout.Interval = TimeoutMinutes * 60 * 1000;
            timerTimeout.Tick    += TimerTimeout_Tick;
            timerTimeout.Start();
        }

        // ── Fecho programático (chamado pelo AfterExecution) ──────────────────

        /// <summary>
        /// Fecha o formulario de forma controlada, mesmo a partir de outra thread.
        /// Seguro chamar mesmo que o form ja esteja fechado/disposed.
        /// </summary>
        public void CloseForm()
        {
            if (this.IsDisposed)
                return;

            if (this.InvokeRequired)
            {
                // BeginInvoke: fire-and-forget para evitar deadlock
                // (a UI thread pode estar bloqueada à espera do AI)
                this.BeginInvoke(new Action(CloseForm));
                return;
            }

            _allowClose = true;
            this.Close();
        }

        // ── Eventos ───────────────────────────────────────────────────────────

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            _allowClose = true;
            this.Close();
        }

        private void TimerTimeout_Tick(object sender, EventArgs e)
        {
            timerTimeout.Stop();
            labelMensagem.Text = "Tempo limite atingido. O processamento pode ter falhado.";

            // Pequena pausa para o utilizador ler a mensagem, depois fecha
            var pauseTimer = new Timer { Interval = 3000 };
            pauseTimer.Tick += (s, _) =>
            {
                ((Timer)s).Stop();
                ((Timer)s).Dispose();
                _allowClose = true;
                if (!this.IsDisposed) this.Close();
            };
            pauseTimer.Start();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!_allowClose && e.CloseReason == CloseReason.UserClosing)
            {
                // Alt+F4 tambem funciona como cancelar
                _allowClose = true;
            }

            base.OnFormClosing(e);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            timerTimeout.Stop();
            timerTimeout.Dispose();
            base.OnFormClosed(e);
        }
    }
}