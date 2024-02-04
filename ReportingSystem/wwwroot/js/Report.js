new Vue({
    el: '#Report',
    data: {
        selectedCategory: '',
        selectedCategory1: '',
        selectedCategory2: '',
        selectedCategory3: '',
        selectedProject: '',
        projects:'',
        project:'',
        selectedSubcategory:'',
        category1: '',
        category2: '',
        category3: '',
        category4: '',
        categories:[],
        categories1:[],
        categories2:[],
        categories3:[],
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
        worksHourAmount: 8,
        worksHourStart: 9,
        breakHour: 1,
        highlightedRow: null,
        cursorDateMonth: new Date().getMonth() + 1,
        cursorDateYear: new Date().getFullYear(),
        cursorDate: new Date(),
        modeScreen: "Місяць",
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
        async GetProject(id) {

        },
        setProject(event) {
            const selectedProjectId = event.target.value;
            // Отримано вибраний проект
            console.log("Selected Project ID:", selectedProjectId);

            // Тепер ви можете використовувати selectedProjectId або викликати інші методи з ним
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
            console.log(responseCategory.data);
        },
        showDetails(day) {
            if (day > 0) {
                this.isShowDetails = !this.isShowDetails;
            }
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
                //this.isShowDetails = false;
                // Вивести дату у консоль при натисканні на комірку
                console.log(`Clicked on cell with date: ${day}-${this.cursorDateMonth}-${this.cursorDateYear}`);
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

                console.log(currentDate);
            } else {
                return null; // Дата не належить до поточного місяця
            }
        },
        handleCellDayClick(hour) {
            let currentDate = new Date(this.cursorDate);

            currentDate.setHours(hour);
            currentDate.setMinutes(0);

            console.log(currentDate);
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
            let responseReports = '';
            responseReports = await axios.get("/Reports/GetReports", {
                params: {
                    idCu: this.selectedCustomerId,
                    idCo: this.selectedCompanyId,
                    idEm: this.selectedEmployeeId,
                    datestart: this.datestart,
                    dateend: this.dateend,
                }
            });
            this.reports = responseReports.data;
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
