﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;


namespace GerencialGraphic
{
    public partial class FGraficoGerencial : Form
    {
        // Parâmetros para geração dos gráficos        
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal {get;set;}
        public string ModuloFiltrado { get; set; }

        public FGraficoGerencial()
        {
            InitializeComponent();            
        }

        private async void FGraficoGerencial_Load(object sender, EventArgs e)
        {
            await webView21.EnsureCoreWebView2Async(null);

            CarregarGrafico();

            await Task.Delay(500);
            await EnviarMesesParaGrafico();
        }

        private async Task EnviarMesesParaGrafico()
        {
            var meses = new List<string>
            {
                "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho" // Meses exemplares
            };


            var valores = new List<int>
            {
                12345, 10000, 20000, 27500, 15000, 8000 // Valores exemplares
            };

            string jsonMeses = System.Text.Json.JsonSerializer.Serialize(meses);
            string jsonValores = System.Text.Json.JsonSerializer.Serialize(valores);

            await webView21.ExecuteScriptAsync($"configurarGrafico('{jsonMeses}', '{jsonValores}')");
        }

        private void CarregarGrafico()
        {
            string caminhoHtml = Path.Combine(Application.StartupPath, "grafico.html");

            if (File.Exists(caminhoHtml))            
                File.Delete(caminhoHtml);

            string script = @"
            <!DOCTYPE html>
            <html lang='pt-br'>
            <head>
                <meta charset='UTF-8'>
                <title>Gráfico Gerencial</title>
                <script src='https://cdn.jsdelivr.net/npm/chart.js'></script>
                <script src='https://cdnjs.cloudflare.com/ajax/libs/jspdf/2.4.0/jspdf.umd.min.js'></script>
                        <style>
                            body { display: flex; align-items: center; justify-content: center; height: 100vh; margin: 0; background-color: #f4f4f4; position: relative; }
                            #meuGrafico { max-width: 800px; max-height: 500px; }
                            #exportButton {
                                position: absolute;
                                top: 20px;
                                right: 20px;
                                background-color: blue;
                                color: white;
                                border: none;
                                padding: 10px 20px;
                                border-radius: 5px;
                                cursor: pointer;
                            }
                            #exportButton:hover {
                                background-color: darkblue;
                            }
                        </style>
                    </head>
                    <body>
                        <button id='exportButton' onclick='exportarPDF()'>Exportar para PDF</button>
                        <canvas id='meuGrafico'></canvas>
                        <script>
                            const ctx = document.getElementById('meuGrafico').getContext('2d');
                            const meuGrafico = new Chart(ctx, {
                                type: 'line',
                                data: {
                                    labels: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio'],
                                    datasets: [{
                                        label: 'Período',
                                        data: [5000, 10000, 20000, 27000, 15000],
                                        backgroundColor: 'rgba(75, 192, 192, 0.2)',
                                        borderColor: 'rgba(75, 192, 192, 1)',
                                        borderWidth: 2,
                                        fill: true
                                    }]
                                },
                                options: {
                                    responsive: true,
                                    scales: {
                                        y: {
                                            beginAtZero: true,
                                            ticks: { stepSize: 5000 }
                                        }
                                    }
                                }
                            });

                            document.getElementById('meuGrafico').addEventListener('contextmenu', function(event) {
                                event.preventDefault();
                            });

                            function configurarGrafico(mesesJson, valoresJson) {
                                const meses = JSON.parse(mesesJson);
                                const valores = JSON.parse(valoresJson);

                                meuGrafico.data.labels = meses;
                                meuGrafico.data.datasets[0].data = valores;

                                meuGrafico.update();
                            }

                            function exportarPDF() {
                                const { jsPDF } = window.jspdf;
                                const pdf = new jsPDF('landscape');

                                const canvas = document.getElementById('meuGrafico');
                                const imgData = canvas.toDataURL('image/png');

                                pdf.addImage(imgData, 'PNG', 10, 10, 280, 150);
                                pdf.save('Gerencial.pdf');
                            }
                        </script>
                    </body>
                    </html>";

            File.WriteAllText(caminhoHtml, script);
            webView21.Source = new Uri(caminhoHtml);
        }      
    }
}
