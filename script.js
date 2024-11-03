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