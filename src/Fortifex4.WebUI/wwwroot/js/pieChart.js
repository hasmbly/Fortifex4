window.PieChart =
{
    pieChart: null,

    init: function (config)
    {
        PieChart.displayChart(config);
    },

    destroy: function ()
    {
        if (PieChart.pieChart != null)
        {
            PieChart.pieChart.destroy();
        }
    },

    displayChart: function (config)
    {
        console.log("JS: config: " + JSON.stringify(config));

        var ctx = document.getElementById(config.pieChartElementID).getContext('2d');

        PieChart.pieChart = new Chart(ctx, {
            type: 'doughnut',
            data: {
                labels: config.labels,
                datasets: [{
                    label: '# of Votes',
                    data: config.data,
                    backgroundColor: config.backgroundColor,
                    borderColor: config.backgroundColor,
                    borderWidth: 1
                }]
            },
            options: {
                responsive: true,
                legend: {
                    position: 'top',
                },
                title: {
                    display: true,
                    text: config.title
                },
                animation: {
                    animateScale: true,
                    animateRotate: true
                }
            }
        });
    },
};