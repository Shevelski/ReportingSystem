new Vue({
    el: '#Rolls',
    data: {
        modalRolActive:'',
        customerId: '',
        companyId: '',
        employeeId: '',
        employees: [{
            rol: {
                rolName: '',
                rolType: -1
            },
            position: {
                namePosition:""
            }
        }],
        showSaveButton: false,
        showSaveIndex: -1,
        showArrayButtonSave:[],
        tmpNameRol: '', 
        isEditMode: false,
        isNewSelectedCustomer: false,
        saveCustomer: false,
        idCustomer: '',
        isNewSelectedCompany: false,
        saveCompany: false,
        idCompany: '',
        selectedRol:'',
        selectedCustomerId: 0,
        selectedCustomerIdCheck: 0,
        selectedCompanyId: 0,
        selectedCompanyIdCheck: 0,
        useOpenDataBot: true,
        showArchive: false,
        searchQuery: '',
        pageCount: 1,
        pageCur: 1,
        itemsPerPage: 10,
        modalType: 0,
        modalIndex: null,
        indexCompany: 0,
        modalName: '',
        modalOperation: null,
        modalTitle: null,
        modalCompanyActive: false,
        editCompanyName: '',
        editEDRPU: '',
        editCompanyAddress: '',
        editCompanyActions: '',
        editCompanyPhone: '',
        editCompanyEmail: '',
        newCompany: [0],
        companies: [0],
        rolls: [{
            rolName: '',
            rolType: -1
        }]
    },
    mounted() {
        this.customerId = document.getElementById('idCu').textContent;
        this.companyId = document.getElementById('idCo').textContent;
        this.employeeId = document.getElementById('idEm').textContent;
        this.rol = document.getElementById('rol').textContent;
        this.Init();
    },
    computed: {
        countFilteredRolls() {
            const nameFilter = this.searchQuery ? this.searchQuery.toLowerCase() : '';

            let filteredList = this.rolls.filter((rol) => {
                const nameMatches = !nameFilter || rol.rolName.toLowerCase().includes(nameFilter);
                
                return nameMatches;
            });

            return filteredList.length;
        },
        filteredRolls() {
            const nameFilter = this.searchQuery ? this.searchQuery.toLowerCase() : '';

            let filteredList = this.rolls.filter((rol) => {
                const nameMatches = !nameFilter || rol.rolName.toLowerCase().includes(nameFilter);
                return nameMatches;
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

            console.log(this.rol);

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

            
            this.rolls = await this.getAllRolls();
            console.log(this.rolls);

            this.pageCount = Math.ceil(this.countFilteredRolls / this.itemsPerPage);
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
        async getAllRolls() {

            if (this.selectedCustomerId == 0) {
                this.selectedCustomerId = this.customers[0].id;
            }

            if (this.selectedCompanyId == 0) {
                this.selectedCompanyId = this.companies[0].id;
            }

            let responseRolls = await axios.get("/Rolls/GetAllRolls", {
                params: {
                    idCu: this.selectedCustomerId,
                    idCo: this.selectedCompanyId,
                    idEm: this.employeeId,
                }
            });
            return responseRolls.data;
        },
        async getEmployeesByRol(rol) {
            this.tmpNameRol = rol;
            this.isEditMode = false;
            let response = await axios.get("/Rolls/GetEmployeesByRoll", {
                params: {
                    idCu: this.selectedCustomerId,
                    idCo: this.selectedCompanyId,
                    rol: rol
                }
            });
            this.showArrayButtonSave.length = 0;
           
            this.employees = response.data;
            console.log(this.employees);
            return response.data;
        },
        getSelectedRol(event,index) {
            this.selectedRol = event.target.value;

            if (this.tmpNameRol != this.selectedRol) {
                this.showArrayButtonSave.push({ key: index, value: true });
            } else {
                this.showArrayButtonSave = this.showArrayButtonSave.filter(item => item.key !== index);
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
        setItemsPerPage(count) {
            this.itemsPerPage = count;
            this.pageCount = Math.ceil(this.countFilteredRolls / this.itemsPerPage);
        },
        nextBatch() {
            this.pageCount = Math.ceil(this.countFilteredRolls / this.itemsPerPage);

            if (this.pageCur < this.pageCount) {
                this.pageCur++;
            }
        },
        prevBatch() {
            if (this.pageCur > 1) {
                this.pageCur--;
            }
        },
        firstBatch() {
            if (this.pageCur !== 1) {
                this.pageCur = 1;
            }
        },
        async editRol(index) {
            const v0 = this.selectedCustomerId;
            const v1 = this.selectedCompanyId;
            const v2 = this.employees[index].id;
            const v3 = this.employees[index].rol.rolName;

            const ar = [v0, v1, v2, v3];

            try {
                await axios.post('/Rolls/EditEmployeeRol', ar);
            } catch (error) {
                console.error('Помилка під час виклику методу EditEmployeeRol:', error);
            }
            this.showArrayButtonSave = this.showArrayButtonSave.filter(item => item.key !== index);

            this.getEmployeesByRol(this.tmpNameRol);
            this.isEditMode = true;
            this.isEditMode = true;
        },

        closeAllAccordions() {
            this.pageCount = Math.ceil(this.countFilteredRolls / this.itemsPerPage);
            const accordionItems = document.querySelectorAll(".accordion-collapse");
            accordionItems.forEach(item => {
                item.classList.remove("show");
            });
        },
    },
});
