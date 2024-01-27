new Vue({
    el: '#Configuration',
    data: {
        hasDatabase: false,
        generationProcess: '',
        databaseStatus: '',
        showButtons: false,
        percentOperation: 0,
        isPageLoaded: false,
        serverName: '',
        databaseName: '',
        newServer: '',
        newDatabase: '',
        status: 0,
        loginDB:'',
        passwordDB:'',
        login: '',
        password: '',
        useCred: false,
        pagePasswordOk: false,
        pagePasswordFailed: false,
        showModal: true,
        username: '',
        isLoggedIn: false
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
            this.hasDatabase = await axios.get("/Home/HasDatabase").data;
            let response = await axios.get("/Home/GetConnectionString");
            this.serverName = response.data[0];
            this.databaseName = response.data[1];
            this.showModal = true;
            console.log("Test");
        },
        openModal() {
            this.showModal = true;
        },
        closeModal() {
            this.showModal = false;
        },
        async loginUser() {
            let result = await axios.get("/Home/ConfigEnter", {
                params: {
                    username: this.username,
                    password: this.password,
                }
            });
            if (result.data) {
                this.pagePasswordOk = true;
                this.isLoggedIn = true;
                this.pagePasswordFailed = false;
            } else {
                this.pagePasswordFailed = true;
            };
        },
        async IsServerAvailable() {
            if (!this.useCred) {
                let result = await axios.get("/Home/IsServerAvailable1", {
                    params: {
                        serverName: this.newServer
                    }
                });
                if (result.data) {
                    this.SetStatus(4);
                } else {
                    this.SetStatus(3);
                }
            } else {
                let result = await axios.get("/Home/IsServerAvailable1", {
                    params: {
                        serverName: this.newServer,
                        login: this.loginDB,
                        password: this.passwordDB
                    }
                });
                if (result.data) {
                    this.SetStatus(4);
                } else {
                    this.SetStatus(3);
                }
            }
        },
        async IsDatabaseAvailable() {
            if (!this.useCred) {
                let result = await axios.get("/Home/IsDatabaseAvailable1", {
                    params: {
                        serverName: this.newServer,
                        databaseName: this.newDatabase
                    }
                });
                if (result.data) {
                    this.SetStatus(6);
                } else {
                    this.SetStatus(5);
                }
            } else {
                let result = await axios.get("/Home/IsDatabaseAvailable2", {
                    params: {
                        serverName: this.newServer,
                        databaseName: this.newDatabase,
                        login: this.loginDB,
                        password: this.passwordDB
                    }
                });
                if (result.data) {
                    this.SetStatus(6);
                } else {
                    this.SetStatus(5);
                }
            }
            
        },
        async IsTablesAvailable() {
            if (!this.useCred) {
                let result = await axios.get("/Home/IsTablesAvailable1", {
                    params: {
                        serverName: this.newServer,
                        databaseName: this.newDatabase
                    }
                });
                if (result.data) {
                    this.SetStatus(8);
                } else {
                    this.SetStatus(7);
                }
            } else {
                let result = await axios.get("/Home/IsTablesAvailable2", {
                    params: {
                        serverName: this.newServer,
                        databaseName: this.newDatabase,
                        login: this.login,
                        password: this.password
                    }
                });
                if (result.data) {
                    this.SetStatus(8);
                } else {
                    this.SetStatus(7);
                }
            }
            
        },
        async SetStatus(status) {
            if (status == 1) {
                this.newServer = this.serverName;
                this.newDatabase = this.databaseName;
            };
            this.status = status;
            console.log("status " + status);
        },
        async useData(typeOfData) {
            this.isDataChosen = true;
            if (typeOfData) {
                this.newServer = this.serverName;
                this.newDatabase = this.databaseName;

                console.log("Власні дані");
            } else {
                this.ownData = false;
                console.log("Нові дані");
            }

            console.log(typeOfData);
        },
        async SetConnectionString() {
         
            if (!this.useCred) {
                var ar = [this.newServer, this.newDatabase, "", ""];
                try {
                    await axios.post('/Home/SetConnectionString', ar);
                } catch (error) {
                    console.error('Помилка під час виклику методу EditCompany:', error);
                }
                
            } else {
                var ar = [this.newServer, this.newDatabase, this.login, this.password];
                try {
                    await axios.post('/Home/SetConnectionString', ar);
                } catch (error) {
                    console.error('Помилка під час виклику методу EditCompany:', error);
                }
            }
        },
        async createDatabase() {
            this.SetStatus(9);
            await this.SetConnectionString();
            await axios.get("/Home/GenerateData");
            this.percentOperation = 0;
            //this.SetStatus(10);

            //this.generationProcess = response.data;
        },
        async createTable() {
            this.SetStatus(9);
            await axios.get("/Home/GenerateData");
            this.percentOperation = 0;
            this.SetStatus(10);

            //this.generationProcess = response.data;
            console.log("Test");
        },
        async cancelCreation() {
            this.percentOperation = 0;
            this.SetStatus(0);
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
