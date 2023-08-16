
new Vue({
    el: '#Employees',
    data: {
        isSelectCompany: false,
        selectedCompanyId: '',
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
        companies: [0]
    },
    mounted() {
        this.Init();
    },
    computed: {
        saveCompany() {

        },
        countFilteredEmployees() {
            const currentDate = new Date();
            const nameFilter = this.searchQuery ? this.searchQuery.toLowerCase() : '';

            let filteredList = this.employees.filter((employee) => {
                const nameMatches = !nameFilter || employee.name.toLowerCase().includes(nameFilter);
                const isInArchive = employee.status.employeeStatusType === 3;

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
                const nameMatches = !nameFilter || employee.name.toLowerCase().includes(nameFilter);
                const isInArchive = employee.status.employeeStatusType === 3;

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
            let responseCompanies = await axios.get("/Companies/GetActualCompanies");
            this.companies = responseCompanies.data;
            console.log(this.companies);
            var idComp = '';

            if (this.isSelectCompany) {
                idComp = this.selectedCompanyId;
            };

            console.log("Selected Company ID:", this.selectedCompanyId);
            console.log("Selected Company ID:", idComp);

            let response = await axios.get("/Employees/GetEmployees", {
                params: {
                    id: idComp
                }
            });

            this.employees = response.data;
            console.log(this.employees);
            this.pageCount = Math.ceil(this.countFilteredEmployees / this.itemsPerPage);
            
        },
        getSelectedCompany(event) {
            console.log('ooooooooooooooooooooo');
            this.selectedCompanyId = event.target.value;
            this.isSelectCompany = true;

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
        //handleAccordionOpen(index) {

        //    console.log(`Accordion at index ${index} is opened.`);

        //    if (this.openAccordionIndex !== -1) {
        //        this.openAccordionIndex = -1;
        //    }

        //    this.openAccordionIndex = index;

        //},
        setIndexEmployee(index) {
            this.indexEmployee = index;
            console.log(index);
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
        },
        showFormatDate(dateString) {
            const options = { day: '2-digit', month: '2-digit', year: 'numeric' };
            const jsDate = new Date(dateString);
            return jsDate.toLocaleDateString('en-GB', options);
        },
        formatDate(dateTimeStr) {
            const dateRegex = /^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}$/;
            if (!dateRegex.test(dateTimeStr)) {
                return '';
            }

            const date = new Date(dateTimeStr);
            const day = date.getDate();
            const month = (date.getMonth() + 1).toString().padStart(2, '0');
            const year = date.getFullYear();

            return `${day}/${month}/${year}`;
        },
        async confirmEditEmployee() {
            const v0 = this.filteredEmployees[this.indexEmployee].id;
            //const v1 = this.editUserFirstName;
            //const v2 = this.editUserSecondName;
            //const v3 = this.editUserThirdName;

            var ar = [v0, v1, v2, v3];

            
            try {
                await axios.post('/Users/EditUser', ar);
            } catch (error) {
                console.error('Помилка під час виклику методу EditUser:', error);
            }

            this.Init();
            this.closeAllAccordions();
        },

        async confirmArchiveEmployee() {
            const v0 = this.filteredEmployees[this.indexEmployee].id;
            var ar = [v0];

            try {
                await axios.post('/Users/ArchiveUser', ar);
            } catch (error) {
                console.error('Помилка під час виклику методу ArchiveUser:', error);
            }

            this.Init();
            this.closeAllAccordions();
        },
        async confirmDeleteEmployee() {
            const v0 = this.filteredEmployees[this.indexEmployee].id;
            var ar = [v0];

            try {
                await axios.post('/Users/DeleteUser', ar);
            } catch (error) {
                console.error('Помилка під час виклику методу DeleteUser:', error);
            }

            this.Init();
            this.closeAllAccordions();
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
            if (type === 2) {
                this.editEmployeeFirstName = this.filteredEmployees[index].firstName;
                this.editEmployeeSecondName = this.filteredEmployees[index].secondName;
                this.editEmployeeThirdName = this.filteredEmployees[index].thirdName;
                this.modalEmployeeActive = false;
                this.modalOperation = 'Ви впевнені, що хочете редагувати співробітника? ' + this.modalName;
                this.modalTitle = 'Редагування співробітника';
                this.editEmployeeName = this.modalName;
            }
            if (type === 3) {
                this.modalName = this.filteredEmployees[index].name;
                if (!this.modalEmployeeActive) {
                    this.modalOperation = 'Ви впевнені, що хочете архівувати співробітника? ' + this.modalName;
                }
                this.modalTitle = 'Архівування співробітника';
            }
            if (type === 4) {
                this.modalName = this.filteredEmployees[index].name;
                if (!this.modalEmployeeActive) {
                    this.modalOperation = 'Ви впевнені, що хочете видалити співробітника? ' + this.modalName;
                }
                this.modalTitle = 'Видалення співробітника';
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
