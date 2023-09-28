
new Vue({
    el: '#Administrators',
    data: {
        newEmployee: {
            firstName: '',
            secondName: '',
            thirdName: '',
            birthDate: '',
            workStartDate: '',
            namePosition:'',
            login: '',
            rol: '',
            password: '',
            phoneWork: '',
            phoneSelf: '',
            emailWork: '',
            emailSelf: '',
            addressReg: '',
            addressFact: '',
            salary: '',
            taxNumber: '',
            addSalary:'',
        },
        customerId: '',
        companyId: '',
        employeeId: '',
        rol:'',
        beforeEditEmployee: '',
        positions: [0],
        rolls:[0],
        selectedRol:'',
        selectedPosition: '',
        isNewSelectedCompany: false,
        saveCompany: false,
        idCompany: '',
        isNewSelectedCustomer: false,
        saveCustomer: false,
        idCustomer: '',
        mode: 'standart',
        showOnlyActualCompany: false,
        showEmployeeInfo: false,
        selectedCompanyId: 0,
        selectedCompanyIdCheck: 0,
        selectedCustomerId: 0,
        selectedCustomerIdCheck: 0,
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
        customers: [0],
        holidayDays: 0,
        hospitalDays: 0,
        assignmentDays: 0,
        taketimeoffDays: 0,
    },
    mounted() {
        this.customerId = document.getElementById('idCu').textContent;
        this.companyId = document.getElementById('idCo').textContent;
        this.employeeId = document.getElementById('idEm').textContent;
        this.rol = document.getElementById('rol').textContent;
        this.Init();
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
           
            /*this.positions = await this.getPositions();*/
            this.rolls = await this.getRolls();
            this.employees = await this.getEmployees();


            console.log(this.employees);
            for (let i = 0; i < this.employees.length; i++) {
                this.employees[i].birthDate = this.dateCSharpToJs(this.employees[i].birthDate);
                this.employees[i].workStartDate = this.dateCSharpToJs(this.employees[i].workStartDate);
                this.employees[i].workEndDate = this.dateCSharpToJs(this.employees[i].workEndDate);
            };

            this.pageCount = Math.ceil(this.countFilteredEmployees / this.itemsPerPage);
        },
        async getRolls() {
            let response = await axios.get("/Companies/GetDevRolls");
            return response.data;
        },
        async getEmployees() {
            let response = await axios.get("/Employees/GetAdministrators");
            return response.data;
        },
        
        isFormEmpty() {
            const employee = this.newEmployee;
            return (
                employee.firstName == '' || employee.firstName == null || employee.firstName == undefined ||
                employee.secondName == '' || employee.secondName == null || employee.secondName == undefined ||
                employee.thirdName == '' || employee.thirdName == null || employee.thirdName == undefined ||
                employee.birthDate == '' || employee.birthDate == null || employee.birthDate == undefined ||
                employee.login == '' || employee.login == null || employee.login == undefined ||
                employee.rol == '' || employee.rol == null || employee.rol == undefined ||
                employee.password == '' || employee.password == null || employee.password == undefined ||
                employee.phoneWork == '' || employee.phoneWork == null || employee.phoneWork == undefined ||
                employee.emailWork == '' || employee.emailWork == null || employee.emailWork == undefined
               )
        },
        
       
        getSelectedRol(event) {
            this.selectedRol = event.target.value;
        },
        //getSelectedPosition(event) {
        //    this.selectedPosition = event.target.value;
        //},
       
        ToogleMode(mode) {
            if (this.mode == 'edit') {
                this.toggleModal(2, this.indexEmployee);
            } else {
                this.beforeEditEmployee = this.filteredEmployees[this.indexEmployee];
            }

            if (this.mode != mode) {
                this.mode = mode;
            }
        },
        async editEmployee() {
            this.ToogleMode('standart');
            try {
                const response = await axios.post('/Employees/EditAdministrator', this.filteredEmployees[this.indexEmployee]);
            } catch (error) {
                console.error('Помилка під час виклику методу EditAdministrator:', error);
            }
            this.Init();
        },
        
        generateGuid() {
            const template = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx';
            return template.replace(/[xy]/g, function (c) {
                const r = Math.random() * 16 | 0;
                const v = c === 'x' ? r : (r & 0x3 | 0x8);
                return v.toString(16);
            });
        },
        setIndexEmployee(index) {
            if (index == this.indexEmployee && this.showEmployeeInfo) {
                this.showEmployeeInfo = false;
            } else {
                this.showEmployeeInfo = true;
                this.indexEmployee = index;
            }
        },
        GeneratePassword(index) {
            const length = 8;
            const charset = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"; // Можна додати інші символи за потреби
            let password = "";

            for (let i = 0; i < length; i++) {
                const randomIndex = Math.floor(Math.random() * charset.length);
                password += charset[randomIndex];
            }

            if (index == -1) {
                this.newEmployee.password = password;
                this.$forceUpdate();
            } else {
                this.filteredEmployees[this.indexEmployee].password = password;
                
            }
            
        },
        shortNameEmployee(index) {

            var formattedName = this.filteredEmployees[index].secondName + ' ';
            if (this.filteredEmployees[index].firstName) {
                formattedName += this.filteredEmployees[index].firstName.charAt(0) + '.';
            }
            if (this.filteredEmployees[index].thirdName) {
                formattedName += this.filteredEmployees[index].thirdName.charAt(0) + '.';
            }
            return formattedName;
        },
        setItemsPerPage(count) {
            this.itemsPerPage = count;
            this.pageCount = Math.ceil(this.countFilteredEmployees / this.itemsPerPage);
        },
        nextBatch(){
            this.pageCount = Math.ceil(this.countFilteredEmployees / this.itemsPerPage);

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
            this.closeAllAccordions();
        },
        hasArchiveEmployee() {
            for (let i = 0; i < this.employees.length; i++) {
                if (this.employees[i].status.employeeStatusType === 2) {
                    return true;
                }
            }
            return false;
        },
        dateCSharpToJs(dateTimeStr) {
            const date = new Date(dateTimeStr);
            const day = date.getDate().toString().padStart(2, '0');
            const month = (date.getMonth() + 1).toString().padStart(2, '0');
            const year = date.getFullYear();

            return `${year}-${month}-${day}`;
        },

        async confirmArchiveEmployee() {
            const idEm = this.filteredEmployees[this.indexEmployee].id;

            const ar = [idEm];

            try {
                await axios.post('/Employees/ArchiveAdministrator', ar);
            } catch (error) {
                console.error('Помилка під час виклику методу ArchiveAdministrator:', error);
            }

            this.Init();
            this.closeAllAccordions();
            this.showEmployeeInfo = !this.showEmployeeInfo;
        },
        async fromArchiveEmployee() {
            const idEm = this.filteredEmployees[this.indexEmployee].id;
            const ar = [idEm];

            try {
                await axios.post('/Employees/FromArchiveAdministrator', ar);
            } catch (error) {
                console.error('Помилка під час виклику методу FromArchiveAdministrator:', error);
            }
            this.Init();
            this.closeAllAccordions();
            this.showEmployeeInfo = !this.showEmployeeInfo;
        },
        async confirmDeleteEmployee() {
            const idEm = this.filteredEmployees[this.indexEmployee].id;
            const ar = [idEm];

            try {
                await axios.post('/Employees/DeleteAdministrator', ar);
            } catch (error) {
                console.error('Помилка під час виклику методу DeleteAdministrator:', error);
            }

            this.Init();
        },
        async checkEmail() {
            var email = this.newEmployee.emailWork;
            if (this.newEmployee.emailWork != null) {
                var response = await axios.get("/Employees/IsBusyEmail", {
                    params: {
                        email: email,
                    }
                });
                console.log(response.data);
            };


            
        },
        async addEmployee() {
            const v0 = this.newEmployee.firstName;
            const v1 = this.newEmployee.secondName;
            const v2 = this.newEmployee.thirdName;
            const v3 = this.newEmployee.birthDate;
            const v4 = this.newEmployee.login;
            const v5 = this.newEmployee.rol.rolName;
            const v6 = this.newEmployee.rol.rolType.toString();
            const v7 = this.newEmployee.password;
            const v8 = this.newEmployee.phoneWork;
            const v9 = this.newEmployee.emailWork;

            const ar = [v0, v1, v2, v3, v4, v5, v6, v7, v8, v9];

            console.log(ar);
            try {
                await axios.post('/Employees/CreateAdministrator', ar);
            } catch (error) {
                console.error('Помилка під час виклику методу CreateAdministrator:', error);
            }
            this.closeAllAccordions();
            this.Init();

        },
        

        toggleModal(type, index) {

            this.modalType = type;
            this.indexEmployee = index;

            if (type === 1) {
                this.modalEmployeeActive = false;
                this.modalOperation = ''
                this.modalTitle = 'Додавання співробітника';
                this.editEmployeeName = this.modalName;
            }

            if (type === 10) {
                this.modalEmployeeActive = false;
                this.modalOperation = 'Ви впевнені, що хочете додати співробітника? ' + this.modalName;
                this.modalTitle = 'Додавання співробітника';
                this.editEmployeeName = this.modalName;
            }

            if (type === 2) {
                this.modalEmployeeActive = false;
                this.modalOperation = 'Ви впевнені, що хочете редагувати співробітника ' + this.beforeEditEmployee.firstName + " " + this.beforeEditEmployee.secondName + " " + this.beforeEditEmployee.thirdName + " ?";
                this.modalTitle = 'Редагування співробітника';
            }
            if (type === 3) {
                if (!this.modalEmployeeActive) {
                    this.modalOperation = 'Ви впевнені, що хочете архівувати співробітника ' + this.filteredEmployees[index].firstName + " " + this.filteredEmployees[index].secondName + " " + this.filteredEmployees[index].thirdName + " ?";
                }
                this.modalTitle = 'Архівування співробітника';
            }
            if (type === 4) {
                if (!this.modalEmployeeActive) {
                    this.modalOperation = 'Ви впевнені, що хочете видалити співробітника ' + this.filteredEmployees[index].firstName + " " + this.filteredEmployees[index].secondName + " " + this.filteredEmployees[index].thirdName + " ?";
                }
                this.modalTitle = 'Видалення співробітника';
            }
            if (type === 5) {
                if (!this.modalEmployeeActive) {
                    this.modalOperation = 'Ви впевнені, що хочете відновити співробітника ' + this.filteredEmployees[index].firstName + " " + this.filteredEmployees[index].secondName + " " + this.filteredEmployees[index].thirdName + " ?";
                }
                this.modalTitle = 'Відновлення співробітника';
            }
        },
        handleImageChange(event) {
            const file = event.target.files[0];
            if (file) {
                const reader = new FileReader();
                reader.onload = (e) => {
                    this.$nextTick(() => {
                        this.updateImage(e.target.result);
                    });
                };
                reader.readAsDataURL(file);
            }
        },
        updateImage(imageData) {
            const imageElement = this.$refs.imageElement;
            if (imageElement) {
                const prefix = 'data:image/jpeg;base64,';
                const imageDataWithoutPrefix = imageData.replace(prefix, '');
                imageElement.src = imageDataWithoutPrefix;
            }
        },
        closeAllAccordions() {
            this.pageCount = Math.ceil(this.countFilteredEmployees / this.itemsPerPage);
            const accordionItems = document.querySelectorAll(".accordion-collapse");
            accordionItems.forEach(item => {
                item.classList.remove("show");
            });
        },
    },
});
