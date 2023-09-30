new Vue({
    el: '#Structure',
    data: {
        customerId: '',
        companyId: '',
        employeeId: '',
        rol:'',
        positions: [0],
        allPositions: [0],
        rolls:[0],
        selectedRol:'',
        selectedPosition: '',
        idCompany: '',
        idCustomer: '',
        mode: 'standart',
        selectedCompanyId: 0,
        selectedCustomerId: 0,
        employees: [0],
        companies: [0],
        customers: [0],
       
    },
    mounted() {
        this.customerId = document.getElementById('idCu').textContent;
        this.companyId = document.getElementById('idCo').textContent;
        this.employeeId = document.getElementById('idEm').textContent;
        this.rol = document.getElementById('rol').textContent;
        this.Init();

        //const dropzone = document.getElementById('dropzone');

        //dropzone.addEventListener('dragover', this.handleDragOver);
        //dropzone.addEventListener('drop', this.handleDrop);
    },
    computed: {
        countFilteredEmployees() {
            const nameFilter = this.searchQuery ? this.searchQuery.toLowerCase() : '';

            let filteredList = this.employees.filter((employee) => {
                const nameMatches = !nameFilter || employee.firstName.toLowerCase().includes(nameFilter) || employee.secondName.toLowerCase().includes(nameFilter) || employee.thirdName.toLowerCase().includes(nameFilter);
                const isInArchive = employee.status && employee.status.employeeStatusType == 2;

                if (this.showArchive) {
                    return isInArchive && nameMatches;
                } else {
                    return !isInArchive && nameMatches;
                }
            });

            return filteredList.length;
        },

        filteredEmployees() {
            const nameFilter = this.searchQuery ? this.searchQuery.toLowerCase() : '';

            let filteredList = this.employees.filter((employee) => {
                const nameMatches = !nameFilter || employee.firstName.toLowerCase().includes(nameFilter) || employee.secondName.toLowerCase().includes(nameFilter) || employee.thirdName.toLowerCase().includes(nameFilter);
                const isInArchive = employee.status && employee.status.employeeStatusType == 2;

                if (this.showArchive) {
                    return isInArchive && nameMatches;
                } else {
                    return !isInArchive && nameMatches;
                }
            });

            if (this.pageCur === 1) {
                return filteredList.slice(0, this.pageCur * this.itemsPerPage);
            } else {
                return filteredList.slice(this.pageCur * this.itemsPerPage - this.itemsPerPage, this.pageCur * this.itemsPerPage);
            }
        },
    },
    methods: {


        async Init() {
            if (this.rol == 'Developer' || this.rol == 'DevAdministrator') {
                await this.updateCustomers();
                await this.updateCompanies();
            }
            if (this.rol == 'Customer') {
                this.selectedCustomerId = this.customerId;
                await this.updateCompanies();
            }
            if (this.rol == 'CEO') {
                this.selectedCustomerId = this.customerId;
                this.selectedCompanyId = this.companyId;
            }
            this.positions = await this.getPositions();
            this.allPositions = await this.getAllPositions();



            this.rolls = await this.getRolls();
            this.employees = await this.getEmployees();


            console.log(this.employees);
            console.log(this.positions);
            console.log(this.allPositions);

        },

        handleDragOver(event) {
            event.preventDefault();
        },

        handleDrop(event) {
            event.preventDefault();
            const itemId = event.dataTransfer.getData('text/plain');
            const item = document.getElementById(itemId);
            event.target.appendChild(item);
        },

        async getPositions() {
            let response = await axios.get("/Positions/GetUniqPositions", {
                params: {
                    idCu: this.selectedCustomerId,
                    idCo: this.selectedCompanyId
                }
            });
            return response.data;
        },
        async getAllPositions() {
            let response = await axios.get("/Positions/GetAllPositionsWithEmployee", {
                params: {
                    idCu: this.selectedCustomerId,
                    idCo: this.selectedCompanyId
                }
            });
            return response.data;
        },
        async getRolls() {
            let response = await axios.get("/Companies/GetRolls", {
                params: {
                    idCu: this.selectedCustomerId,
                    idCo: this.selectedCompanyId
                }
            });
            return response.data;
        },
        async getEmployees() {
            let response = await axios.get("/Employees/GetEmployees", {
                params: {
                    idCu: this.selectedCustomerId,
                    idCo: this.selectedCompanyId
                }
            });
            return response.data;
        },
        async updateCompanies() {
            let responseCompanies = '';
            responseCompanies = await axios.get("/Companies/GetCompanies", {
                params: {
                    idCu: this.selectedCustomerId,
                }
            });
            this.companies = responseCompanies.data;
            if (this.selectedCompanyId == 0) {
                this.selectedCompanyId = this.companies[0].id;
            }
            this.IsNewSelectedCompany = false;
        },
        async updateCustomers() {
            let responseCustomers = await axios.get("/Customers/GetCustomers");
            this.customers = responseCustomers.data;

            if (this.selectedCustomerId == 0) {
                this.selectedCustomerId = this.customers[0].id;
            }
        },
        
        getSelectedCustomer(event) {
            this.selectedCustomerId = event.target.value;

            if (this.selectedCustomerIdCheck !== this.selectedCustomerId) {
                this.selectedCompanyId = 0;
                this.IsNewSelectedCustomer = true;
                this.saveCustomer = false;
            } else {
                this.saveCustomer = true;
            }
            this.Init();
        },
        getSelectedCompany(event) {
            this.selectedCompanyId = event.target.value;
            
            if (this.selectedCompanyIdCheck !== this.selectedCompanyId) {
                this.IsNewSelectedCompany = true;
                this.saveCompany = false;
                
            } else {
                this.saveCompany = true;
            }

            this.Init();
        },
        getSelectedRol(event) {
            this.selectedRol = event.target.value;
        },
        getSelectedPosition(event) {
            this.selectedPosition = event.target.value;
        },
    },
});