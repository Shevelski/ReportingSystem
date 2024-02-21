new Vue({
    el: '#Configuration',
    data: {
        mode: 1,
        hasDatabase: false,
        generationProcess: '',
        databaseStatus: '',
        showButtons: false,
        percentOperation: 0,
        isPageLoaded: false,
        serverName: '',
        databaseName: '',
        isUseDatabaseCredential: false,
        databaseLogin: '',
        databasePassword: '',
        newServerName: '',
        newDatabaseName: '',
        newIsUseDatabaseCredential: false,
        newDatabaseLogin: '',
        newDatabasePassword: '',
        status: 0,
        login: '',
        password: '',
        useCred: '',
        pagePasswordOk: false,
        pagePasswordFailed: false,
        showModal: true,
        username: '',
        isLoggedIn: false,
        cust: '',
        isServerOk: false,
        isDatabaseOk: false,
        isTablesOk: false,
        isServerLoad: false,
        isDatabaseLoad: false,
        isTablesLoad: false,
        databaseCreated: false,
        databaseCreateLoad: false,
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
            //this.hasDatabase = await axios.get("/Home/HasDatabase").data;
            let response = await axios.get("/Home/GetConnectionString");
            this.serverName = response.data[0];
            this.databaseName = response.data[1];
            this.isUseDatabaseCredential = response.data[2] == "True" ? true : false;
            this.databaseLogin = response.data[3];
            this.databasePassword = response.data[4];
            this.StartTest(); 
            this.showModal = true;
            //this.openModalProgrammatically();
        },
        StartTest() {
            this.IsServerAvailable();
            this.IsDatabaseAvailable();
            this.IsTablesAvailable();
        },
        openModal() {
            this.showModal = true;
        },
        closeModal() {
            this.showModal = false;
        },
        resetConnection() {
            this.isServerOk = false;
            this.isDatabaseOk = false;
            this.isTablesOk = false;
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
                this.closeModalProgrammatically();
            } else {
                this.pagePasswordFailed = true;
            };
        },
        async IsServerAvailable() {
            this.isServerLoad = true;
            this.isServerOk = false;
            this.isDatabaseOk = false;
            this.isTablesOk = false;
            this.isServerLoad = true;
            let result = '';
            if (this.mode == 1) {
                result = await axios.get("/Home/IsServerAvailable", {
                    params: {
                        serverName: this.serverName,
                        databaseName: this.databaseName,
                        isUseDatabaseCredential: this.isUseDatabaseCredential ? 'True' : 'False',
                        login: this.databaseLogin,
                        password: this.databasePassword
                    }
                });
            } else {
                result = await axios.get("/Home/IsServerAvailable", {
                    params: {
                        serverName: this.newServerName,
                        databaseName: this.newDatabaseName,
                        isUseDatabaseCredential: this.newIsUseDatabaseCredential ? 'True' : 'False',
                        login: this.newDatabaseLogin,
                        password: this.newDatabasePassword
                    }
                });
            }
            if (result.data) {
                this.isServerOk = true;
            };
            this.isServerLoad = false;
        },
        async IsDatabaseAvailable() {
            this.isDatabaseLoad = true;
            this.isDatabaseOk = false;
            this.isTablesOk = false;
            let result = '';
            if (this.mode == 1) {
                result = await axios.get("/Home/IsDatabaseAvailable", {
                    params: {
                        serverName: this.serverName,
                        databaseName: this.databaseName,
                        isUseDatabaseCredential: this.isUseDatabaseCredential ? 'True' : 'False',
                        login: this.databaseLogin,
                        password: this.databasePassword
                    }
                });
            } else {
                result = await axios.get("/Home/IsDatabaseAvailable", {
                    params: {
                        serverName: this.newServerName,
                        databaseName: this.newDatabaseName,
                        isUseDatabaseCredential: this.newIsUseDatabaseCredential ? 'True' : 'False',
                        login: this.newDatabaseLogin,
                        password: this.newDatabasePassword
                    }
                });
            }
            
            if (result.data) {
                this.isServerOk = true;
                this.isDatabaseOk = true;
            };
            this.isDatabaseLoad = false;
            //this.SetConnectionString();
        },
        async IsTablesAvailable() {
            this.isTablesLoad = true;
            this.isTablesOk = false;
            let result = '';
            if (this.mode == 1) {
                result = await axios.get("/Home/IsTablesAvailable", {
                    params: {
                        serverName: this.serverName,
                        databaseName: this.databaseName,
                        isUseDatabaseCredential: this.isUseDatabaseCredential ? 'True' : 'False',
                        login: this.databaseLogin,
                        password: this.databasePassword
                    }
                });
            } else {
                result = await axios.get("/Home/IsTablesAvailable", {
                    params: {
                        serverName: this.newServerName,
                        databaseName: this.newDatabaseName,
                        isUseDatabaseCredential: this.newIsUseDatabaseCredential ? 'True' : 'False',
                        login: this.newDatabaseLogin,
                        password: this.newDatabasePassword
                    }
                });
            }
            if (result.data) {
                this.isServerOk = true;
                this.isDatabaseOk = true;
                this.isTablesOk = true;
            };
            this.isTablesLoad = false;
        },
        async SetConnectionString() {

            var ar = [];
            
            if (this.mode == 1) {
                ar = [this.serverName, this.databaseName, this.isUseDatabaseCredential ? "True" : "False", this.databaseLogin, this.databasePassword]
            } else {
                ar = [this.newServerName, this.newDatabaseName, this.newIsUseDatabaseCredential ? "True" : "False", this.newDatabaseLogin, this.newDatabasePassword]
            }

            try {
                await axios.post('/Home/SetConnectionString', ar);
            } catch (error) {
                console.error('Помилка під час виклику методу SetConnectionString:', error);
            }
        },
        async createDatabase() {
            this.databaseCreateLoad = true;
            await this.SetConnectionString();
            this.databaseCreated = await axios.get("/Home/GenerateData");
            this.databaseCreateLoad = false;
            this.percentOperation = 0;
        },
        closeModalProgrammatically() {
            let modalElement = document.getElementById('exampleModal');
            if (modalElement) {
                modalElement.dispatchEvent(new Event('click'));
            }
        },
        openModalProgrammatically() {
            if (typeof bootstrap !== 'undefined' && typeof bootstrap.Modal === 'function') {
                let modalElement = new bootstrap.Modal(document.getElementById('exampleModal'));
                modalElement.show();
            } else {
                this.waitForBootstrap(() => {
                    let modalElement = new bootstrap.Modal(document.getElementById('exampleModal'));
                    modalElement.show();
                });
            }
        },

        waitForBootstrap(callback) {
            const interval = setInterval(() => {
                if (typeof bootstrap !== 'undefined' && typeof bootstrap.Modal === 'function') {
                    clearInterval(interval);
                    callback();
                }
            }, 100);
        },
        setupSignalR() {
            this.connection = new signalR.HubConnectionBuilder().withUrl("/statusHub").build();
            this.connection.on("ReceiveStatus", (status, percent) => {
                this.databaseStatus = status;
                this.percentOperation = percent;        
            });
            this.connection.on("ReceiveStatus1", (customer) => {
                this.cust = customer;
            });
            this.connection.start().then(() => {
                console.log("Connected to SignalR Hub");
            }).catch((err) => {
                return console.error(err.toString());
            });
        }
    }
})
