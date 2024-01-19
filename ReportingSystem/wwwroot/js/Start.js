new Vue({
    el: '#Start',
    data: {
        hasDatabase: false,
        generationProcess: '',
        databaseStatus: '',
        showButtons: false,
        percentOperation: 0,
        isPageLoaded: false
    },
    mounted() {
        this.Init();
        this.setupSignalR();
        this.isPageLoaded = true;
    },
    computed: {
    },
    methods: {
        async Init() {
            this.showButtons = true;
            let response = await axios.get("/Home/HasDatabase");
            this.hasDatabase = response.data;
            console.log("Test");
        },
        async createDatabase() {
            await axios.get("/Home/GenerateData");
            this.showButtons = false;
            this.percentOperation = 0;

            //this.generationProcess = response.data;
            console.log("Test");
        },
        async cancelCreation() {
            this.showButtons = false;
            this.percentOperation = 0;
        },
        setupSignalR() {
            // Створіть з'єднання SignalR
            this.connection = new signalR.HubConnectionBuilder().withUrl("/statusHub").build();

            // При отриманні повідомлення від сервера
            this.connection.on("ReceiveStatus", (status, percent) => {
                this.databaseStatus = status;  // Оновіть відображення статусу на сторінці
                this.percentOperation = percent;  // Оновіть відображення статусу на сторінці
            });

            // Запуск підключення SignalR
            this.connection.start().then(() => {
                console.log("Connected to SignalR Hub");
            }).catch((err) => {
                return console.error(err.toString());
            });
        }
    }
})
