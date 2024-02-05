new Vue({
    el: '#Report',
    data: {
        curday:'',
        selectedHours: 0,
        comment: '',
        projectsDay: '',
        reportsDay:'',
        isUsePeriod: false,
        selectedCategory: '',
        selectedCategory1: '',
        selectedCategory2: '',
        selectedCategory3: '',
        selectedProject: '',
        projects: '',
        project: '',
        selectedSubcategory: '',
        category1: '',
        category2: '',
        category3: '',
        category4: '',
        categories: [],
        categories1: [],
        categories2: [],
        categories3: [],
        dateTimeInput1: '',
        dateTimeInput2: '',
        currentMonth: new Date().getMonth() + 1,
        currentYear: new Date().getFullYear(),
        selectedDate: new Date(),
        hovered: false,
        is24HourFormat: true,
        isRowHighlighted: false,
        isWorkHours: true,
        isWorkDays: true,
        isWeekDay: true,
        isShowDetails: false,
        worksHourAmount: 7,
        worksHourStart: 9,
        breakHour: 1,
        highlightedRow: null,
        cursorDateMonth: new Date().getMonth() + 1,
        cursorDateYear: new Date().getFullYear(),
        cursorDate: new Date(),
        modeScreen: "Місяць",
        modalRolActive: '',
        customerId: '',
        companyId: '',
        employeeId: '',
        employees: [{
            rol: {
                rolName: '',
                rolType: -1
            },
            position: {
                namePosition: ""
            }
        }],
        showSaveButton: false,
        showSaveIndex: -1,
        showArrayButtonSave: [],
        tmpNameRol: '',
        isEditMode: false,
        isNewSelectedCustomer: false,
        saveCustomer: false,
        idCustomer: '',
        isNewSelectedCompany: false,
        saveCompany: false,
        idCompany: '',
        selectedRol: '',
        selectedCustomerId: 0,
        selectedCustomerIdCheck: 0,
        selectedEmployeeId: 0,
        selectedEmployeeIdCheck: 0,
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
        this.cursorDate = new Date();
        this.customerId = document.getElementById('idCu').textContent;
        this.companyId = document.getElementById('idCo').textContent;
        this.employeeId = document.getElementById('idEm').textContent;
        this.rol = document.getElementById('rol').textContent;
        this.Init();
    },
    computed: {
        currentDate() {
            const now = new Date();
            return {
                day: now.getDate(),
                month: now.getMonth() + 1,
                year: now.getFullYear()
            };
        },
        weeks() {
            let firstDayOfMonth = new Date(this.cursorDate.getFullYear(), this.cursorDate.getMonth(), 1).getDay();
            firstDayOfMonth = (firstDayOfMonth === 0) ? 6 : firstDayOfMonth - 1;
            const daysInMonth = new Date(this.cursorDate.getFullYear(), this.cursorDate.getMonth() + 1, 0).getDate();
            let d1;
            if (firstDayOfMonth <= 5) {
                d1 = 5;
            } else {
                if (daysInMonth > 31) {
                    d1 = 6;
                } else {
                    d1 = 5;
                }
            }
            let dayCounter = 1;
            let calendar = [];

            for (let i = 0; i < d1; i++) {
                let week = [];
                for (let j = 0; j < 7; j++) {
                    if ((i === 0 && j < firstDayOfMonth) || dayCounter > daysInMonth) {
                        week.push('');
                    } else {
                        week.push(dayCounter++);
                    }
                }
                calendar.push(week);
            }
            return calendar;
        },
    },
    methods: {
        async Init() {

            console.log(this.rol);

            if (this.rol == 'Developer' || this.rol == 'DevAdministrator') {
                await this.updateCustomers();
                await this.updateCompanies();
                await this.updateEmployees();
            }
            if (this.rol == 'Customer') {
                this.selectedCustomerId = this.customerId;
                await this.updateCompanies();
                await this.updateEmployees();
            }
            if (this.rol == 'CEO') {
                this.selectedCustomerId = this.customerId;
                this.selectedCompanyId = this.companyId;
                this.selectedEmployeeId = this.employeeId;
            }
            this.rolls = await this.getAllRolls();
            this.getCategories();
            
            console.log("cat = " + this.categories);
        },
        UsePeriod() {
            this.isUsePeriod = !this.isUsePeriod;
        },
        UsePeriod1() {
            // Перетворення значення selectedHours на числовий тип
            const hoursToAdd = parseInt(this.selectedHours, 10);

            this.dateTimeInput2 = new Date(this.dateTimeInput1);
            console.log("s0 " + hoursToAdd);

            // Додавання годин до dateTimeInput2
            this.dateTimeInput2.setHours(this.dateTimeInput2.getHours() + hoursToAdd);

            console.log("s1 " + this.dateTimeInput1);
            console.log("s2 " + this.dateTimeInput2);
        },
       // getReportProject(hour) {
       //     const reportForHour = this.reports.find(report => {
       //         const reportStartDate = new Date(report.startDate);
       //         const reportEndDate = new Date(report.endDate);
       //         const reportStartHour = reportStartDate.getHours();
       //         const reportEndHour = reportEndDate.getHours();

       //         return hour >= reportStartHour && hour <= reportEndHour;
       //     });

       //     let name = "";
       //reportsDay

       //     return reportForHour ? reportForHour.idProject : "";
        // },
        getReportProject(hour) {
            const reportForHour = this.reports.find(report => {
                const reportStartDate = new Date(report.startDate);
                const reportEndDate = new Date(report.endDate);
                const reportStartHour = reportStartDate.getHours();
                const reportEndHour = reportEndDate.getHours();

                return hour >= reportStartHour && hour < reportEndHour;
            });

            if (reportForHour && reportForHour.idProject) {
                const project = this.projectsDay.find(project => project.id === reportForHour.idProject);

                if (project) {
                    return project.name;
                } else {
                    let category = "";
                    const category0 = this.categories.find(category => category.id === reportForHour.idCategory0);
                    
                    if (category0 != null && category0 != "" && category0 != "'00000000-0000-0000-0000-000000000000'") {
                        category = category0.name;
                        const category1 = this.categories.find(category => category0.categoriesLevel1.id === reportForHour.idCategory1);
                        if (category1 != null && category1 != "" && category1 != "'00000000-0000-0000-0000-000000000000'") {
                            category = category1.name;
                            const category2 = this.categories.find(category => category1.categoriesLevel2.id === reportForHour.idCategory2);
                            if (category2 != null && category2 != "" && category2 != "'00000000-0000-0000-0000-000000000000'") {
                                category = category2.name;
                                const category3 = this.categories.find(category => category2.categoriesLevel3.id === reportForHour.idCategory3);
                                if (category3 != null && category3 != "" && category3 != "'00000000-0000-0000-0000-000000000000'") {
                                    category = category3.name;
                                }
                            }
                        }
                    }
                    return category;
                }
            }

            return "";
        },
        getReportComment(hour) {
            const reportForHour = this.reports.find(report => {
                const reportStartDate = new Date(report.startDate);
                const reportEndDate = new Date(report.endDate);
                const reportStartHour = reportStartDate.getHours();
                const reportEndHour = reportEndDate.getHours();

                return hour >= reportStartHour && hour < reportEndHour;
            });

            return reportForHour ? reportForHour.comment : "";
        },
        async SendReport() {
            var v0 = this.selectedCustomerId;
            var v1 = this.selectedCompanyId;
            var v2 = this.selectedEmployeeId;
            var v3 = this.dateTimeInput1;
            var v4 = this.dateTimeInput2;
            var v5 = this.selectedCategory;
            var v6 = this.selectedCategory1;
            var v7 = this.selectedCategory2;
            var v8 = this.selectedCategory3;
            var v9 = this.selectedProjectId;
            var v10 = this.comment;

            var ar = [v0, v1, v2, v3, v4, v5, v6, v7, v8, v9, v10];

            try {
                await axios.post('/Report/SendReport', ar);
            } catch (error) {
                console.error('Помилка під час виклику методу SendReport:', error);
            }
            this.closeModalProgrammatically();
            this.showDetails(this.curday);
        },
        async GetProjects(ids) {
            let responseProject = '';
            let projects = [];
            for (var i = 0; i < ids.length; i++) {
                responseProject = await axios.get("/Projects/GetProject", {
                    params: {
                        idCu: this.selectedCustomerId,
                        idCo: this.selectedCompanyId,
                        idPr: ids[i]
                    }
                });
                let project = responseProject.data;
                projects.push(project);
            }
            return projects;
        },
        async GetProjectName(id) {
            let responseProject = '';
            responseProject = await axios.get("/Projects/GetProject", {
                params: {
                    idCu: this.selectedCustomerId,
                    idCo: this.selectedCompanyId,
                    idPr: id
                }
            });
            let project = responseProject.data;
            console.log("project = " + project);
            return project.name;
        },
        setProject(event) {
            this.selectedProjectId = event.target.value;
        },
        setCat0() {
            this.projects = null;
            console.log(this.selectedCategory);
            console.log(this.categories);
            for (var i = 0; i < this.categories.length; i++) {
                if (this.categories[i].id == this.selectedCategory) {
                    this.categories1 = this.categories[i].categoriesLevel1;
                    this.categories2 = null;
                    this.categories3 = null;
                    if (this.categories[i].projects != null && this.categories[i].projects.length > 0) {
                        let promise = this.GetProjects(this.categories[i].projects);
                        promise.then(result => {
                            this.projects = result;
                        });
                    }
                };
            }
            console.log(this.projects);
        },
        setCat1() {
            this.projects = null;
            console.log(this.selectedCategory1);
            for (var i = 0; i < this.categories1.length; i++) {
                if (this.categories1[i].id == this.selectedCategory1) {
                    this.categories2 = this.categories1[i].categoriesLevel2;
                    this.categories3 = null;
                    if (this.categories1[i].projects != null && this.categories1[i].projects.length > 0) {
                        let promise = this.GetProjects(this.categories1[i].projects);
                        promise.then(result => {
                            this.projects = result;
                        });
                    }
                };
            }
        },
        setCat2() {
            this.projects = null;
            for (var i = 0; i < this.categories2.length; i++) {
                if (this.categories2[i].id == this.selectedCategory2) {
                    this.categories3 = this.categories2[i].categoriesLevel3;
                    if (this.categories2[i].projects != null && this.categories2[i].projects.length > 0) {
                        let promise = this.GetProjects(this.categories2[i].projects);
                        promise.then(result => {
                            this.projects = result;
                        });
                    }
                };
            }
        },
        setCat3() {
            this.projects = null;
            if (this.categories3.projects != null && this.categories3.projects.length > 0) {
                this.projects = this.categories3.projects;
                if (this.categories3[i].projects != null && this.categories3[i].projects.length > 0) {
                    let promise = this.GetProjects(this.categories3[i].projects);
                    promise.then(result => {
                        this.projects = result;
                    });
                }
            }
        },
        async getCategories() {
            let responseCategory = '';
            responseCategory = await axios.get("/ProjectsCategories/GetCategories", {
                params: {
                    idCu: this.selectedCustomerId,
                    idCo: this.selectedCompanyId
                }
            });
            this.categories = responseCategory.data;
            console.log(this.categories);
        },
        showDetails(day) {
            if (day > 0) {
                this.isShowDetails = true;
            }
        },
        async getProjects() {
            let response = await axios.get("/Projects/GetProjects", {
                params: {
                    idCu: this.selectedCustomerId,
                    idCo: this.selectedCompanyId
                }
            });
            this.projectsDay = response.data;
        },
        isSelectedDay(day) {
            return day === this.selectedDate.getDate() &&
                this.selectedDate.getMonth() + 1 === this.cursorDate.getMonth() + 1 &&
                this.selectedDate.getFullYear() === this.cursorDate.getFullYear();
        },
        isCurrentDay(day) {
            return day === this.currentDate.day &&
                this.cursorDate.getMonth() + 1 === this.currentDate.month &&
                this.cursorDate.getFullYear() === this.currentDate.year;
        },
        isHourInWorkRange(hour) {
            return hour >= this.worksHourStart && hour < this.worksHourStart + this.worksHourAmount;
        },
        highlightRow(hour) {
            this.isRowHighlighted = true;
            this.highlightedRow = hour;
        },
        resetHighlight() {
            this.isRowHighlighted = false;
            this.highlightedRow = null;
        },
        handleCellClick(day) {
            if (day > 0) {
                const dateString = `${day}-${this.cursorDate.getMonth() + 1}-${this.cursorDate.getFullYear()}`;
                const [dayPart, monthPart, yearPart] = dateString.split('-');
                const dateObject = new Date(`${yearPart}-${monthPart}-${dayPart}`);
                this.cursorDate = dateObject;
                this.selectedDate = this.cursorDate;
                console.log(`Clicked on cell with date: ${day}-${this.cursorDateMonth}-${this.cursorDateYear}`);
                this.getReports();
                this.getProjects();
                this.curday = day;
            }
        },
        handleCellWeekClick(hour, numDayOfWeek) {
            let currentDate = new Date(this.cursorDate);
            let currentDay = currentDate.getDate();
            let currentDayOfWeek = currentDate.getDay();

            let difference = numDayOfWeek - currentDayOfWeek;
            currentDate.setDate(currentDay + difference);

            // Перевірка, чи нова дата належить до поточного місяця
            if (currentDate.getMonth() === this.cursorDate.getMonth()) {
                // Додаємо години та хвилини до обраної дати
                currentDate.setHours(hour);
                currentDate.setMinutes(0);

                this.dateTimeInput1 = new Date(currentDate);
                this.dateTimeInput2 = new Date(this.dateTimeInput1);
                this.dateTimeInput2.setHours(this.dateTimeInput2.getHours() + 1);
                console.log("w1 " + this.dateTimeInput1);
                console.log("w2 " + this.dateTimeInput2);
                this.openModalProgrammatically();

            } else {
                return null; // Дата не належить до поточного місяця
            }
        },
        handleCellDayClick(hour) {

            let currentDate = new Date(this.cursorDate);

            currentDate.setHours(hour);
            currentDate.setMinutes(0);

            this.dateTimeInput1 = new Date(currentDate);
            this.dateTimeInput2 = new Date(this.dateTimeInput1);
            this.dateTimeInput2.setHours(this.dateTimeInput2.getHours() + 1);

            this.openModalProgrammatically();

            console.log("d1 " + this.dateTimeInput1);
            console.log("d2 " + this.dateTimeInput2);
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
        closeModalProgrammatically() {
            let modalElement = document.getElementById('exampleModal');
            if (modalElement) {
                modalElement.dispatchEvent(new Event('click'));
            }
        },
        dataDayOfWeek(numDayOfWeek) {
            let currentDate = new Date(this.cursorDate);
            let currentDay = currentDate.getDate();
            let currentDayOfWeek = currentDate.getDay();

            let difference = numDayOfWeek - currentDayOfWeek;
            currentDate.setDate(currentDay + difference);

            // Перевірка, чи нова дата належить до поточного місяця
            if (currentDate.getMonth() === this.cursorDate.getMonth()) {
                // Отримуємо день і місяць у форматі dd.mm
                let day = currentDate.getDate().toString().padStart(2, '0');
                let month = (currentDate.getMonth() + 1).toString().padStart(2, '0');
                return `${day}.${month}`;
            } else {
                return null; // Дата не належить до поточного місяця
            }
        },
        updateDateTimeInput1(event) {
            const localDate = new Date(event.target.value);
            const offsetMinutes = localDate.getTimezoneOffset();
            console.log("Local date:", localDate);
            console.log("Offset minutes:", offsetMinutes);
            localDate.setMinutes(localDate.getMinutes() - offsetMinutes);
            this.dateTimeInput1 = localDate.toISOString().slice(0, 16);
            console.log("Updated dateTimeInput1:", this.dateTimeInput1);
        },
        updateDateTimeInput2(event) {
            const localDate = new Date(event.target.value);
            const offsetMinutes = localDate.getTimezoneOffset();
            console.log("Local date:", localDate);
            console.log("Offset minutes:", offsetMinutes);
            localDate.setMinutes(localDate.getMinutes() - offsetMinutes);
            this.dateTimeInput2 = localDate.toISOString().slice(0, 16);
            console.log("Updated dateTimeInput2:", this.dateTimeInput2);
        },
        formatDate(dateTime) {
            if (!dateTime) return "";

            const localDate = new Date(dateTime);
            const offsetMinutes = localDate.getTimezoneOffset();

            // Коригуємо дату враховуючи різницю в хвилинах
            localDate.setMinutes(localDate.getMinutes() - offsetMinutes);

            return localDate.toISOString().slice(0, 16);
        },
        formatTime(hour, is24HourFormat) {
            if (is24HourFormat) {
                return `${hour}:00`;
            } else {
                const formattedHour = hour % 12 || 12;
                const period = hour < 12 ? 'AM' : 'PM';
                return `${formattedHour}:00 ${period}`;
            }
        },
        convDate(date) {
            var x = new Date(date.getFullYear(), date.getMonth(), 1).toLocaleString('default', { month: 'long' });
            return (date.getFullYear() + " " + x.charAt(0).toUpperCase() + x.slice(1) + " " + date.getDate());
        },
        dateLeft() {
            let currentDate = new Date(this.cursorDate);
            currentDate.setMonth(currentDate.getMonth() - 1);
            this.cursorDate = currentDate;
            console.log(this.cursorDate);
        },
        dateRight() {
            let currentDate = new Date(this.cursorDate);
            currentDate.setMonth(currentDate.getMonth() + 1);
            this.cursorDate = currentDate;
            console.log(this.cursorDate);
        },
        setToday() {
            this.cursorDate = new Date();
            this.selectedDate = new Date();
            this.weeks();

        },
        toggleHover() {
            this.hovered = !this.hovered;
        },
        setMode(period) {
            this.mode = period;
            console.log(period);
        },
        async getReports() { 
            let date = new Date(this.cursorDate);

            // Додаємо 9 годин
            let dateWith9Hours = new Date(date);
            //dateWith9Hours.setHours(dateWith9Hours.getHours() + 9);
            dateWith9Hours.setHours(0, 0, 0, 0);

            // Додаємо 18 годин
            let dateWith18Hours = new Date(date);
            //dateWith18Hours.setHours(dateWith18Hours.getHours() + 18);
            dateWith18Hours.setHours(23, 59, 59, 999);
            let responseReports = await axios.get("/Report/GetReports", {
                params: {
                    idCu: this.selectedCustomerId,
                    idCo: this.selectedCompanyId,
                    idEm: this.selectedEmployeeId,
                    datestart: dateWith9Hours,
                    dateend: dateWith18Hours,
                }
            });
            this.reports = responseReports.data; 
            console.log(this.reports);
        },
        async setReports() { 

        },
        async updateEmployees() {
            let responseEmployees = '';
            responseEmployees = await axios.get("/Employees/GetEmployees", {
                params: {
                    idCu: this.selectedCustomerId,
                    idCo: this.selectedCompanyId,
                }
            });
            this.employees = responseEmployees.data;
            if (this.selectedEmployeeId == 0) {
                this.selectedEmployeeId = this.employees[0].id;
            }
            this.IsNewSelectedEmployee = false;
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
        getSelectedEmployee(event) {
            this.selectedEmployeeId = event.target.value;

            if (this.selectedEmployeeIdCheck !== this.selectedEmployeeId) {
                this.IsNewSelectedEmployee = true;
                this.saveEmployee = false;

            } else {
                this.saveEmployee = true;
            }

            this.Init();
        },
    },
});
