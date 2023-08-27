﻿
new Vue({
    el: '#Employees',
    data: {
        beforeEditEmployee: '',
        positions: [0],
        rolls:[0],
        selectedRol:'',
        selectedPosition: '',
        isNewSelected: false,
        saveCompany: false,
        idCompany: '',
        mode: 'standart',
        showOnlyActualCompany: false,
        showEmployeeInfo: false,
        selectedCompanyId: 0,
        selectedCompanyIdCheck: 0,
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
                responseCompanies = await axios.get("/Companies/GetActualCompanies");
            } else {
                responseCompanies = await axios.get("/Companies/GetCompanies");
            }
            this.companies = responseCompanies.data;
            console.log(this.companies);

            if (!this.IsNewSelected) {
                var ar = await axios.get("/Companies/CheckSave");
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
                    id: this.selectedCompanyId
                }
            });
            this.positions = responsePositions.data;

            let responseRolls = await axios.get("/Companies/GetRolls", {
                params: {
                    id: this.selectedCompanyId
                }
            });
            this.rolls = responseRolls.data;

            let response = await axios.get("/Employees/GetEmployees", {
                params: {
                    id: this.selectedCompanyId
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
        getSelectedRol(event) {
            this.selectedRol = event.target.value;
        },
        getSelectedPosition(event) {
            this.selectedPosition = event.target.value;
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
        ToogleMode(mode) {
            if (this.mode === 'edit') {
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
                const response = await axios.post('/Employees/EditEmployee', this.filteredEmployees[this.indexEmployee]);
            } catch (error) {
                console.error('Помилка під час виклику методу EditEmployee:', error);
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
        GeneratePassword() {
            const length = 8;
            const charset = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"; // Можна додати інші символи за потреби
            let password = "";

            for (let i = 0; i < length; i++) {
                const randomIndex = Math.floor(Math.random() * charset.length);
                password += charset[randomIndex];
            }

            this.filteredEmployees[this.indexEmployee].password = password;
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
        holidayDaysCount(indexEmployee) {
            const currentYear = new Date().getFullYear();
            if (this.employees[indexEmployee].holidayDate !== null) {
                const datesThisYear = this.employees[indexEmployee].holidayDate.filter(date => {
                    return new Date(date).getFullYear() === currentYear;
                });
                return datesThisYear.length;
            } else {
                return 0;
            }   
        },
        hospitalDaysCount(indexEmployee) {
            const currentYear = new Date().getFullYear();
            if (this.employees[indexEmployee].hospitalDate !== null) {
                const datesThisYear = this.employees[indexEmployee].hospitalDate.filter(date => {
                    return new Date(date).getFullYear() === currentYear;
                });
                return datesThisYear.length;
            } else {
                return 0;
            }
        },
        assignmentDaysCount(indexEmployee) {
            const currentYear = new Date().getFullYear();
            if (this.employees[indexEmployee].assignmentDate !== null) {
                const datesThisYear = this.employees[indexEmployee].assignmentDate.filter(date => {
                    return new Date(date).getFullYear() === currentYear;
                });
                return datesThisYear.length;
            } else {
                return 0;
            }
        },
        taketimeoffDaysCount(indexEmployee) {
            const currentYear = new Date().getFullYear();
            if (this.employees[indexEmployee].taketimeoffDate !== null) {
                const datesThisYear = this.employees[indexEmployee].taketimeoffDate.filter(date => {
                    return new Date(date).getFullYear() === currentYear;
                });
                return datesThisYear.length;
            } else {
                return 0;
            }
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
        dateCSharpToJs(dateTimeStr) {
            const date = new Date(dateTimeStr);
            const day = date.getDate().toString().padStart(2, '0');
            const month = (date.getMonth() + 1).toString().padStart(2, '0');
            const year = date.getFullYear();

            return `${year}-${month}-${day}`;
        },
        formatDateToISO(date) {
            const year = date.getFullYear();
            const month = String(date.getMonth() + 1).padStart(2, '0');
            const day = String(date.getDate()).padStart(2, '0');
            const hours = String(date.getHours()).padStart(2, '0');
            const minutes = String(date.getMinutes()).padStart(2, '0');
            const seconds = String(date.getSeconds()).padStart(2, '0');
            const offset = -date.getTimezoneOffset();
            const offsetHours = Math.floor(offset / 60);
            const offsetMinutes = offset % 60;
            const offsetSign = offset >= 0 ? '-' : '+';

            return `${year}-${month}-${day}T${hours}:${minutes}:${seconds}${offsetSign}${String(offsetHours).padStart(2, '0')}:${String(offsetMinutes).padStart(2, '0')}`;
        },
        async confirmEditEmployee() {
            const v0 = this.filteredEmployees[this.indexEmployee].id;

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
                this.modalEmployeeActive = false;
                this.modalOperation = 'Ви впевнені, що хочете редагувати співробітника ' + this.beforeEditEmployee.firstName + " " + this.beforeEditEmployee.secondName + " " + this.beforeEditEmployee.thirdName + " ?";
                this.modalTitle = 'Редагування співробітника';

                //this.beforeEditEmployee = this.filteredEmployees[this.indexEmployee];

                //for (const key in this.beforeEditEmployee) {
                //    if (this.beforeEditEmployee.hasOwnProperty(key)) {
                //        if (this.beforeEditEmployee[key] !== this.filteredEmployees[index][key]) {
                //            console.log(`Key: ${key}, Before: ${this.beforeEditEmployee[key]}, After: ${this.filteredEmployees[index][key]}`);
                //        }
                //    }
                //}
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
