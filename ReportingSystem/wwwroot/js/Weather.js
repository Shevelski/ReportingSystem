new Vue({
    el: '#Weather',
    data: {
        data: {},
        selectedCity: '',
        dataCity: {},
        chartData: {}
    },
    mounted() {
        this.Get();
    },
    methods: {
        async Get(type) {
            let response = await axios.get("/Weather/Cities");
            this.data = response.data;
            this.selectedCity = this.data[0].name;
            this.dataCity = this.data[0];
            console.log(this.dataCity);
            this.renderChart();
        },
        getSelectedCity(event) {
            this.selectedCity = event.target.value;
            this.dataCity = this.data.find(city => city.name === this.selectedCity);
            console.log(this.dataCity);
            this.renderChart();
        },

        renderChart() {
            console.log(this.dataCity);
            if (!this.dataCity || !this.dataCity.weather) {
                console.error('Weather data is not available.');
                return;
            };

            const labels = this.dataCity.weather.dateTime.map(dateTime => {
                const formattedDate = new Date(dateTime).toLocaleString('uk-UA', {
                    year: 'numeric',
                    month: '2-digit',
                    day: '2-digit',
                    hour: 'numeric',
                    minute: 'numeric'
                });
                return formattedDate.replace(/,/, '');
            });

            const currentTime = new Date().toLocaleString('uk-UA', {
                year: 'numeric',
                month: '2-digit',
                day: '2-digit',
                hour: 'numeric'
            }).replace(/,/, '');

            // Add annotation for the current time
            //this.chartData.annotations = [{
            //    type: 'line',
            //    mode: 'vertical',
            //    scaleID: 'x',
            //    value: currentTime,
            //    borderColor: 'green',
            //    borderWidth: 2,
            //    label: {
            //        enabled: true,
            //        content: 'Current Time',
            //        position: 'top'
            //    }
            //}];


            const temperatureData = this.dataCity.weather.temperature;
            const windSpeedData = this.dataCity.weather.windspeed;
            const rainData = this.dataCity.weather.rain;

            this.chartData.labels = labels;
            this.chartData.datasets =  [
                    {
                        label: 'Температура, °C',
                        data: temperatureData,
                        type: 'line',
                        backgroundColor: 'red',
                        borderColor: 'red',
                        borderWidth: 1,
                        yAxidID: 'y1'
                    },
                    //{
                    //    label: 'Швидкість вітру, м/с',
                    //    data: windSpeedData,
                    //    type: 'line',
                    //    backgroundColor: 'white',
                    //    borderColor: 'white',
                    //    borderWidth: 1,
                    //    yAxidID: 'y3'
                    //},
                    {
                        label: 'Рівень опадів, мм',
                        data: rainData,
                        type: 'bar',
                        backgroundColor: 'blue',
                        borderColor: 'blue',
                        borderWidth: 1,
                        yAxisID: 'y2',
                    }
                ];

            const ctx = document.getElementById('myChart').getContext('2d');

            if (this.chart) {
                this.chart.destroy();
            }

            this.chart = new Chart(ctx, {
                type: 'line',
                data: this.chartData,
                options: {
                    scales: {
                        y1: {
                            id: 'y1',
                            type: 'linear',
                            position: 'left',
                            ticks: {
                                color: 'red',
                            },
                        },
                        ////y2: {
                        ////    id: 'y2',
                        ////    type: 'linear',
                        ////    position: 'left',
                        ////    ticks: {
                        ////        color: 'white',
                        ////    },
                        ////},
                        y2: {
                            id: 'y2',
                            type: 'linear',
                            position: 'left',
                            ticks: {
                                color: 'blue',
                            },
                        },
                        x: {
                            id:'x',
                            ticks: {
                                color: 'white',
                            },
                        },
                    },
                    plugins: {
                        annotation: {
                            annotations: {
                                verticalLine: {
                                    type: 'line',
                                    mode: 'vertical',
                                    scaleID: 'x',
                                    value: currentTime,
                                    borderColor: 'red',
                                    borderWidth: 2,
                                    label: {
                                        content: 'Поточний час',
                                        enabled: true,
                                        position: 'top'
                                    }
                                }
                            }
                        }
                    }
                },
            });
        },

    }
});