
new Vue({
    el: '#EmployeesHeaderDev',
    data: {
        items: 0,
        customerId: '',
        companyId: '',
        employeeId: '',
        beforeEditEmployee: '',
        positions: [0],
        rolls:[0],
        selectedRol:'',
        selectedPosition: '',
        isNewSelected: false,
        isNewSelectedCustomer: false,
        saveCompany: false,
        saveCustomer: false,
        idCompany: '',
        mode: 'standart',
        showOnlyActualCompany: false,
        showEmployeeInfo: false,
        selectedCompanyId: 0,
        selectedCompanyIdCheck: 0,
        selectedCustomerId: 0,
        selectedCustomerIdCheck: 0,
        selectedCompanyName: "Виберіть компанію",
        useOpenDataBot: true,
        showArchive: false,
        searchQuery: '',
        pageCount: 1,
        pageCur: 1,
        itemsPerPage: 10,
        modalType: 0,
        modalIndex: null,
        indexEmployee: 0,
        modalName: '',
        modalOperation: null,
        modalTitle: null,
        modalEmployeeActive: false,
        editEmployeeFirstName: '',
        editEmployeeSecondName: '',
        editEmployeeThirdName: '',
        newEmployee: {},
        employees: [0],
        companies: [0],
        holidayDays: 0,
        hospitalDays: 0,
        assignmentDays: 0,
        taketimeoffDays: 0,
    },
    mounted() {
        this.customerId = document.getElementById('Model1').textContent;
        this.companyId = document.getElementById('Model2').textContent;
        this.employeeId = document.getElementById('Model3').textContent;
        console.log(this.customerId);
        console.log(this.companyId);
        console.log(this.employeeId);

        this.Init();
    },
    computed: {
        countFilteredEmployees() {
            const currentDate = new Date();
            const nameFilter = this.searchQuery ? this.searchQuery.toLowerCase() : '';

            let filteredList = this.employees.filter((employee) => {
                const nameMatches = !nameFilter || employee.firstName.toLowerCase().includes(nameFilter) || employee.secondName.toLowerCase().includes(nameFilter) || employee.thirdName.toLowerCase().includes(nameFilter);
                const isInArchive = employee.status.employeeStatusType == 3;

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
                const isInArchive = employee.status.employeeStatusType == 3;

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

            let responseCompanies = '';
            if (this.showOnlyActualCompany) {
                responseCompanies = await axios.get("/Companies/GetActualCompanies", {
                    params: {
                        idCu: this.customerId,
                        idCo: this.selectedCompanyId
                    }
                });
            } else {
                responseCompanies = await axios.get("/Companies/GetCompanies", {
                    params: {
                        idCu: this.customerId,
                        idCo: this.selectedCompanyId
                    }
                });
            }
            this.companies = responseCompanies.data;
            console.log(this.companies);

            if (!this.IsNewSelected) {
                var ar = await axios.get("/Companies/CheckSave", {
                    params: {
                        idCu: this.customerId,
                    }
                });
                this.selectedCompanyId = ar.data;
                if (ar.data == '00000000-0000-0000-0000-000000000000') {
                    this.saveCompany = false;
                    this.selectedCompanyId = this.companies[0].id;
                } else {
                    this.saveCompany = true;
                    this.selectedCompanyId = ar.data;
                    this.selectedCompanyIdCheck = ar.data;
                }
                this.IsNewSelected = true;
            }

            let responsePositions = await axios.get("/Companies/GetPositions", {
                params: {
                    idCu: this.customerId,
                    idCo: this.selectedCompanyId
                }
            });
            this.positions = responsePositions.data;

            let responseRolls = await axios.get("/Companies/GetRolls", {
                params: {
                    idCu: this.customerId,
                    idCo: this.selectedCompanyId
                }
            });
            this.rolls = responseRolls.data;

            let response = await axios.get("/Employees/GetEmployees", {
                params: {
                    idCu: this.customerId,
                    idCo: this.selectedCompanyId
                }
            });

            this.employees = response.data;
            console.log(this.employees);
            for (let j = 0; j < this.employees.length; j++) {
                this.employees[j].birthDate = this.dateCSharpToJs(this.employees[j].birthDate);
                this.employees[j].workStartDate = this.dateCSharpToJs(this.employees[j].workStartDate);
                this.employees[j].workEndDate = this.dateCSharpToJs(this.employees[j].workEndDate);
            };            
            
            this.pageCount = Math.ceil(this.countFilteredEmployees / this.itemsPerPage);
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
                this.IsNewSelected = true;
                this.saveCompany = false;
            } else {
                this.saveCompany = true;
            }
            this.Init();
        },
        async SavePermanentCompany() {
            var id = '';
            if (this.saveCompany) {
                id = this.selectedCompanyId;
                this.selectedCompanyIdCheck = this.selectedCompanyId;
            } else {
                id = '';
                this.selectedCompanyIdCheck = 0;
            };

            console.log(this.saveCompany + ' ' + id);

            try {
                const response = await axios.post('/Companies/SavePermanentCompany', id, {
                    headers: {
                        'Content-Type': 'application/json',
                    },
                });
            } catch(error) {
                console.error('Error calling SavePermanentCompany method:', error);
            }
        },

        async SavePermanentCustomer() {
            var id = '';
            if (this.saveCustomer) {
                id = this.selectedCustomerId;
                this.selectedCustomerIdCheck = this.selectedCustomerId;
            } else {
                id = '';
                this.selectedCustomerIdCheck = 0;
            };

            console.log(this.saveCustomer + ' ' + id);

            try {
                const response = await axios.post('/Customers/SavePermanentCustomer', id, {
                    headers: {
                        'Content-Type': 'application/json',
                    },
                });
            } catch (error) {
                console.error('Error calling SavePermanentCustomer method:', error);
            }
        },
        
        async confirmCreateEmployee() {
            const v0 = this.editEmployeeFirstName;
            const v1 = this.editEmployeeSecondName;
            const v2 = this.editEmployeeThirdName;
           
            var ar = [v0, v1, v2];
            console.log('create');
            try {
                await axios.post('/Users/CreateUser', ar);
            } catch (error) {
                console.error('Помилка під час виклику методу CreateUser:', error);
            }
            this.Init();
            this.closeAllAccordions();
        },

        toggleModal(type, index) {

            this.modalType = type;
            this.indexEmployee = index;

            if (type === 1) {
                this.modalEmployeeActive = false;
                this.modalOperation = 'Ви впевнені, що хочете додати співробітника? ' + this.modalName;
                this.modalTitle = 'Створення нової компанії';
                this.editEmployeeName = this.modalName;
            }
        },
    },
});
