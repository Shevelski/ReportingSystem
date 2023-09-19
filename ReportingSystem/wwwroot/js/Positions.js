new Vue({
    el: '#Positions',
    data: {
        modalPositionActive:'',
        editPositionName:'',
        customerId: '',
        companyId: '',
        employeeId: '',
        employees: [{
            position: {
                position: {
                    namePosition: ''
                }
            }
        }],
        showSaveButton: false,
        showSaveIndex: -1,
        showArrayButtonSave:[],
        tmpNamePosition: '', 
        isEditMode: false,
        isNewSelectedCustomer: false,
        saveCustomer: false,
        idCustomer: '',
        isNewSelectedCompany: false,
        saveCompany: false,
        idCompany: '',
        selectedPosition:'',
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
        positions: [0]
    },
    mounted() {
        this.customerId = document.getElementById('idCu').textContent;
        this.companyId = document.getElementById('idCo').textContent;
        this.employeeId = document.getElementById('idEm').textContent;
        this.rol = document.getElementById('rol').textContent;
        this.Init();
    },
    computed: {
        countFilteredPositions() {
            const nameFilter = this.searchQuery ? this.searchQuery.toLowerCase() : '';

            let filteredList = this.positions.filter((position) => {
                const nameMatches = !nameFilter || position.namePosition.toLowerCase().includes(nameFilter);
                
                return nameMatches;
            });

            return filteredList.length;
        },
        filteredPositions() {
            const nameFilter = this.searchQuery ? this.searchQuery.toLowerCase() : '';

            let filteredList = this.positions.filter((position) => {
                const nameMatches = !nameFilter || position.namePosition.toLowerCase().includes(nameFilter);
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
            
            this.positions = await this.getUniqPositions();
            console.log(this.positions);

            this.pageCount = Math.ceil(this.countFilteredPositions / this.itemsPerPage);
        },
        isPositionUniq() {
            if (this.editPositionName !== null) {
                this.modalPositionActive = false;
                for (var position of this.positions) {
                    if (this.editPositionName === position.namePosition) {
                        this.modalPositionActive = true;
                        this.modalOperation = 'Посада вже існує';
                        break;
                    }
                    this.modalOperation = 'Ви впевнені, що введена посада - коректна? '
                }
            }
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
            let responseCustomers = await axios.get("/Customers/GetAllLicence");
            this.customers = responseCustomers.data;

            if (this.selectedCustomerId == 0) {
                this.selectedCustomerId = this.customers[0].id;
            }
        },
        async getUniqPositions() {
            let responsePositions = await axios.get("/Positions/GetUniqPositions", {
                params: {
                    idCu: this.selectedCustomerId,
                    idCo: this.selectedCompanyId
                }
            });
            return responsePositions.data;
        },
        async getEmployeesByPosition(position) {
            this.tmpNamePosition = position;
            this.isEditMode = false;
            let response = await axios.get("/Positions/GetEmployeesByPosition", {
                params: {
                    idCu: this.selectedCustomerId,
                    idCo: this.selectedCompanyId,
                    pos: position
                }
            });
            this.showArrayButtonSave.length = 0;
            //console.log(position);
            //console.log(response.data);
            this.employees = response.data; 
            return response.data;
        },
        getSelectedPosition(event,index) {
            this.selectedPosition = event.target.value;

            if (this.tmpNamePosition != this.selectedPosition) {
                this.showArrayButtonSave.push({ key: index, value: true });
            } else {
                this.showArrayButtonSave = this.showArrayButtonSave.filter(item => item.key !== index);
            }
        },
        getSelectedCustomer(event) {
            this.selectedCustomerId = event.target.value;

            if (this.selectedCustomerIdCheck !== this.selectedCustomerId) {
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
            this.pageCount = Math.ceil(this.countFilteredPositions / this.itemsPerPage);
        },
        nextBatch() {
            this.pageCount = Math.ceil(this.countFilteredPositions / this.itemsPerPage);

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

        async confirmCreatePosition() {
            const v0 = this.selectedCustomerId;
            const v1 = this.selectedCompanyId;
            const v2 = this.editPositionName;
            const ar = [v0, v1, v2]; 
            try {
                await axios.post('/Positions/CreatePosition', ar);
            } catch (error) {
                console.error('Помилка під час виклику методу CreatePosition:', error);
            }
            this.Init();
            this.closeAllAccordions();
        },

        async confirmEditPosition() {
            const v0 = this.selectedCustomerId;
            const v1 = this.selectedCompanyId;
            const v2 = this.filteredPositions[this.indexPosition].namePosition;
            const v3 = this.editPositionName;
            const ar = [v0, v1, v2, v3];

            try {
                await axios.post('/Positions/EditPosition', ar);
            } catch (error) {
                console.error('Помилка під час виклику методу EditPosition:', error);
            }
            this.Init();
            this.closeAllAccordions();
        },

        async confirmDeletePosition() {
            const v0 = this.selectedCustomerId;
            const v1 = this.selectedCompanyId;
            const v2 = this.editPositionName;
            const ar = [v0, v1, v2];
            try {
                await axios.post('/Positions/DeletePosition', ar);
            } catch (error) {
                console.error('Помилка під час виклику методу DeletePosition:', error);
            }
            this.Init();
            this.closeAllAccordions();
        },

        async editPosition(index) {
            const v0 = this.selectedCustomerId;
            const v1 = this.selectedCompanyId;
            const v2 = this.employees[index].id;
            const v3 = this.employees[index].position.namePosition

            const ar = [v0, v1, v2, v3];

            try {
                await axios.post('/Positions/EditEmployeePosition', ar);
            } catch (error) {
                console.error('Помилка під час виклику методу EditEmployeePosition:', error);
            }
            this.showArrayButtonSave = this.showArrayButtonSave.filter(item => item.key !== index);

            this.getEmployeesByPosition(this.tmpNamePosition);
            this.isEditMode = true;
            //this.Init();
            //this.closeAllAccordions();
        },

        toggleModal(type, index) {

            this.modalType = type;
            this.indexPosition = index;

            if (type === 1) {
                this.modalCompanyActive = false;
                this.modalOperation = 'Ви впевнені, що хочете додати нову посаду ' + this.modalName + ' ?';
                this.modalTitle = 'Додавання нової посади';
                this.editPositionName = this.modalName;
            }
            if (type === 2) {
                this.editPositionName = this.filteredPositions[index].namePosition;
                this.modalName = this.filteredPositions[index].namePosition;
                this.modalCompanyActive = false;
                this.modalOperation = 'Ви впевнені, що хочете перейменувати посаду ' + this.modalName + ' ?';
                this.modalTitle = 'Перейменування посади';
                this.editPositionName = this.modalName;
            }
            
            if (type === 4) {
                this.modalName = this.filteredPositions[index].namePosition;
                this.editPositionName = this.filteredPositions[index].namePosition;
                this.modalPositionActive = this.employees.length > 0;
                if (!this.modalPositionActive) {
                    this.modalOperation = 'Ви впевнені, що хочете видалити посаду ' + this.modalName + ' ?';
                } else {
                    this.modalOperation = 'Співробітники займають дану позицію - ' + this.modalName + '. Необхідно перевести співробітників на інші посади або звільнити співробітників.';
                }
                this.modalTitle = 'Видалення посади';
            }
        },
        closeAllAccordions() {
            this.pageCount = Math.ceil(this.countFilteredPositions / this.itemsPerPage);
            const accordionItems = document.querySelectorAll(".accordion-collapse");
            accordionItems.forEach(item => {
                item.classList.remove("show");
            });
        },
    },
});
