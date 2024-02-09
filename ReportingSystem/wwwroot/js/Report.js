﻿new Vue({
    el: '#Report',
    data: {
        curday: '',
        selectedHours: 1,
        selectedDays: 0,
        comment: '',
        projectsDay: '',
        reportsDay:'',
        projectsWeek: '',
        reportsWeek:'',
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
        console.log(this.cursorDate);
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
        },
        UsePeriod() {
            this.isUsePeriod = !this.isUsePeriod;
        },
        UsePeriod1() {
            const hoursToAdd = parseInt(this.selectedHours, 10);
            this.dateTimeInput2 = new Date(this.dateTimeInput1);
            let currentDate = new Date(this.dateTimeInput1);
            let hour18 = new Date(currentDate.setHours(18, 0, 0, 0));
            let nextDate = new Date(this.dateTimeInput1);
            nextDate.setHours(this.dateTimeInput1.getHours() + hoursToAdd);

            if (nextDate < hour18) {
                this.dateTimeInput2.setHours(this.dateTimeInput2.getHours() + hoursToAdd);
            } else {
                currentDate.setHours(18, 0, 0, 0);
                this.dateTimeInput2 = currentDate;
            };
        },
        getReportProject(hour) {
            if (this.reports != undefined) {
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

                            for (var i = 0; i < category0.categoriesLevel1.length; i++) {
                                if (category0.categoriesLevel1[i].id  === reportForHour.idCategory1) {
                                    const category1 = category0.categoriesLevel1[i];
                                    category = category1.name;
                                    for (var i1 = 0; i1 < category1.categoriesLevel2.length; i1++) {
                                        if (category1.categoriesLevel2[i1].id === reportForHour.idCategory2) {
                                            const category2 = category1.categoriesLevel2[i1];
                                            category = category2.name;
                                            for (var i2 = 0; i2 < category2.categoriesLevel3.length; i2++) {
                                                if (category2.categoriesLevel3[i2].id === reportForHour.idCategory3) {
                                                    const category3 = category2.categoriesLevel3[i2];
                                                    category = category3.name;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        return category;
                    }
                }

                return "";
            }
        },
        getReportProject1(hour, day) {
            if (this.reportsWeek != undefined && this.reportsWeek.length > 0) {

                let currentDate = new Date(this.cursorDate);
                let currentDayOfWeek = currentDate.getDay();

                let difference = day - currentDayOfWeek;
                let startOfWeek = new Date(currentDate);
                startOfWeek.setDate(currentDate.getDate() + difference - (currentDayOfWeek < day ? 7 : 0));

                let endOfWeek = new Date(startOfWeek);
                endOfWeek.setDate(startOfWeek.getDate() + 6);

                const reportForHour = this.reportsWeek.find(report => {
                    const reportStartDate = new Date(report.startDate);
                    const reportEndDate = new Date(report.endDate);
                    const reportStartHour = reportStartDate.getHours();
                    const reportEndHour = reportEndDate.getHours();
                    let reportStartDay = reportStartDate.getDay();
                    let reportEndDay = reportEndDate.getDay();
                    if (reportStartDay == 0) {
                        reportStartDay = 7
                    };
                    if (reportEndDay == 0) {
                        reportEndDay = 7
                    };
                    // Перевірка, чи година потрапляє в інтервал годин
                    const isHourInRange = hour >= reportStartHour && hour < reportEndHour;

                    // Перевірка, чи день тижня потрапляє в інтервал між startOfWeek і endOfWeek
                    const isDayInRange = reportStartDay == day && reportEndDay == day;

                    return isHourInRange && isDayInRange;
                });

                if (reportForHour && reportForHour.idProject && this.projectsDay.length > 0) {
                    const project = this.projectsDay.find(project => project.id === reportForHour.idProject);

                    if (project) {
                        return project.name;
                    } else {
                        let category = "";
                        const category0 = this.categories.find(category => category.id === reportForHour.idCategory0);

                        if (category0 != null && category0 != "" && category0 != "'00000000-0000-0000-0000-000000000000'") {
                            category = category0.name;

                            for (var i = 0; i < category0.categoriesLevel1.length; i++) {
                                if (category0.categoriesLevel1[i].id  === reportForHour.idCategory1) {
                                    const category1 = category0.categoriesLevel1[i];
                                    category = category1.name;
                                    for (var i1 = 0; i1 < category1.categoriesLevel2.length; i1++) {
                                        if (category1.categoriesLevel2[i1].id === reportForHour.idCategory2) {
                                            const category2 = category1.categoriesLevel2[i1];
                                            category = category2.name;
                                            for (var i2 = 0; i2 < category2.categoriesLevel3.length; i2++) {
                                                if (category2.categoriesLevel3[i2].id === reportForHour.idCategory3) {
                                                    const category3 = category2.categoriesLevel3[i2];
                                                    category = category3.name;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        return category;
                    }
                }

                return "";
            }
        },
        getReportComment(hour) {
            
            if (this.reports != undefined) {
                const reportForHour = this.reports.find(report => {
                    const reportStartDate = new Date(report.startDate);
                    const reportEndDate = new Date(report.endDate);
                    const reportStartHour = reportStartDate.getHours();
                    const reportEndHour = reportEndDate.getHours();

                    return hour >= reportStartHour && hour < reportEndHour;
                });

                return reportForHour ? reportForHour.comment : "";
            }

           
        },
        async DeleteDayReport() {
            this.dateTimeInput1 = new Date(this.dateTimeInput1);
            this.dateTimeInput1.setHours(0, 0, 0, 0);
            this.dateTimeInput2 = new Date(this.dateTimeInput1);
            this.dateTimeInput2.setHours(23, 59, 59, 999);

            var v0 = this.selectedCustomerId;
            var v1 = this.selectedCompanyId;
            var v2 = this.selectedEmployeeId;
            var v3 = this.dateTimeInput1;
            var v4 = this.dateTimeInput2;
            var ar = [v0, v1, v2, v3, v4];

            try {
                await axios.post('/Report/ClearDayReport', ar);
            } catch (error) {
                console.error('Помилка під час виклику методу ClearDayReport:', error);
            }
            this.closeModalProgrammatically();
            this.getReports();
            this.getWeekReports();
            this.getProjects();
            this.getCategories();
            this.showDetails(this.curday);

        },
        updateCursorDate(event) {
            console.log("Selected date:", event.target.value);
            this.cursorDate = new Date(event.target.value);
            this.handleCellClick(this.cursorDate.getDate());
        },
        getLocalDate(date) {
            const dateObject = new Date(date.getTime() - date.getTimezoneOffset() * 60000);
            return dateObject.toISOString().split('T')[0];
        },
        async DeleteWeekReport() {

            // Встановлення дати початку на понеділок
            let currentDate = new Date(this.cursorDate); 
            this.dateTimeInput1 = new Date(currentDate);
            this.dateTimeInput1.setHours(0, 0, 0, 0);
            this.dateTimeInput1.setDate(currentDate.getDate() - currentDate.getDay() + 1);

            // Встановлення дати кінця на неділю
            this.dateTimeInput2 = new Date(currentDate);
            this.dateTimeInput2.setHours(23, 59, 59, 999);
            this.dateTimeInput2.setDate(currentDate.getDate() - currentDate.getDay() + 7);


            var v0 = this.selectedCustomerId;
            var v1 = this.selectedCompanyId;
            var v2 = this.selectedEmployeeId;
            var v3 = this.dateTimeInput1;
            var v4 = this.dateTimeInput2;
            var ar = [v0, v1, v2, v3, v4];

            try {
                await axios.post('/Report/ClearDayReport', ar);
            } catch (error) {
                console.error('Помилка під час виклику методу ClearDayReport:', error);
            }
            this.closeModalProgrammatically();
            this.getReports();
            this.getWeekReports();
            this.getProjects();
            this.getCategories();
            this.showDetails(this.curday);

        },
        async DeleteReport() {
            var v0 = this.selectedCustomerId;
            var v1 = this.selectedCompanyId;
            var v2 = this.selectedEmployeeId;
            var v3 = this.dateTimeInput1;
            var v4 = this.dateTimeInput2;
            var ar = [v0, v1, v2, v3, v4];

            try {
                await axios.post('/Report/ClearReport', ar);
            } catch (error) {
                console.error('Помилка під час виклику методу ClearReport:', error);
            }
            this.closeModalProgrammatically();
            this.getReports();
            this.getWeekReports();
            this.getProjects();
            this.getCategories();
            this.showDetails(this.curday);
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
            this.getReports();
            this.getWeekReports();
            this.getProjects();
            this.getCategories();
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
            this.selectedHours = 1;
            this.selectedProjectId = null;
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
            this.selectedHours = 1;
            this.selectedProjectId = null;
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
            this.selectedHours = 1;
            this.selectedProjectId = null;
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
            this.selectedHours = 1;
            this.selectedProjectId = null;
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


                console.log("1= " + day);
                console.log("2= " + this.cursorDate);
                console.log("3= " + this.selectedDate);

                this.getReports();
                this.getProjects();
                this.getCategories();

                this.getWeekReports();
                this.curday = day;
                this.showDetails(day);
            }
        },
        handleCellWeekClick(hour, numDayOfWeek) {

            this.selectedCategory = "";
            this.selectedCategory0 = "";
            this.selectedCategory1 = "";
            this.selectedCategory2 = "";
            this.selectedCategory3 = "";
            this.selectedProject = "";
            this.categories1 = "";
            this.categories2 = "";
            this.categories3 = "";
            this.comment = "";
            this.projectSelect = "";

            let currentDate = new Date(this.cursorDate);
            let currentDay = currentDate.getDate();
            let currentDayOfWeek = currentDate.getDay();

            if (currentDayOfWeek == 0) {
                currentDayOfWeek = 7;
            };
            let difference = numDayOfWeek - currentDayOfWeek;
            currentDate.setDate(currentDay + difference);
            currentDate.setHours(hour);
            currentDate.setMinutes(0);

            let targetReport = this.reportsWeek.find(report => {
                let reportDate = new Date(report.startDate);
                return reportDate.getTime() === currentDate.getTime();
            });

            console.log(targetReport);

            if (targetReport) {

                this.dateTimeInput1 = new Date(targetReport.startDate);
                this.dateTimeInput2 = new Date(targetReport.endDate);

                this.selectedCategory = targetReport.idCategory0;
                this.selectedCategory1 = targetReport.idCategory1;
                this.selectedCategory2 = targetReport.idCategory2;
                this.selectedCategory3 = targetReport.idCategory3;
                this.selectedProject = targetReport.idProject;
                this.comment = targetReport.comment;
                this.setCat0();
                if (this.selectedCategory1 && this.selectedCategory1 !== "00000000-0000-0000-0000-000000000000") {
                    this.setCat1();
                    if (this.selectedCategory2 && this.selectedCategory2 !== "00000000-0000-0000-0000-000000000000") {
                        this.setCat2();
                        if (this.selectedCategory3 && this.selectedCategory3 !== "00000000-0000-0000-0000-000000000000") {
                            this.setCat3();
                        }
                    }
                }
                if (this.selectedProject && this.selectedProject !== "00000000-0000-0000-0000-000000000000") {
                    this.selectedProjectId = this.selectedProject;
                }
                this.openModalProgrammatically();
            } else {
                if (currentDate.getMonth() === this.cursorDate.getMonth()) {
                    currentDate.setHours(hour);
                    currentDate.setMinutes(0);

                    this.dateTimeInput1 = new Date(currentDate);
                    this.dateTimeInput2 = new Date(this.dateTimeInput1);
                    this.dateTimeInput2.setHours(this.dateTimeInput2.getHours() + 1);
                    this.openModalProgrammatically();

                } else {
                    return null;
                }
            }

            //if (currentDate.getMonth() === this.cursorDate.getMonth()) {
            //    currentDate.setHours(hour);
            //    currentDate.setMinutes(0);

            //    this.dateTimeInput1 = new Date(currentDate);
            //    this.dateTimeInput2 = new Date(this.dateTimeInput1);
            //    this.dateTimeInput2.setHours(this.dateTimeInput2.getHours() + 1);
            //    this.openModalProgrammatically();

            //} else {
            //    return null;
            //}
        },
        handleCellDayClick(hour) {
            let currentDate = new Date(this.cursorDate);

            this.selectedCategory = "";
            this.selectedCategory0 = "";
            this.selectedCategory1 = "";
            this.selectedCategory2 = "";
            this.selectedCategory3 = "";
            this.selectedProject = "";
            this.categories1 = "";
            this.categories2 = "";
            this.categories3 = "";

            this.comment = "";
            this.projectSelect = "";
            currentDate.setHours(hour);
            currentDate.setMinutes(0);

            let targetReport = this.reports.find(report => {
                let reportDate = new Date(report.startDate);
                    return reportDate.getTime() === currentDate.getTime();
            });

            if (targetReport) {

                this.dateTimeInput1 = new Date(targetReport.startDate);
                this.dateTimeInput2 = new Date(targetReport.endDate);

                this.selectedCategory = targetReport.idCategory0;
                this.selectedCategory1 = targetReport.idCategory1;
                this.selectedCategory2 = targetReport.idCategory2;
                this.selectedCategory3 = targetReport.idCategory3;
                this.selectedProject = targetReport.idProject;
                this.comment = targetReport.comment;
                this.setCat0();
                if (this.selectedCategory1 && this.selectedCategory1 !== "00000000-0000-0000-0000-000000000000") {
                    this.setCat1();
                    if (this.selectedCategory2 && this.selectedCategory2 !== "00000000-0000-0000-0000-000000000000") {
                        this.setCat2();
                        if (this.selectedCategory3 && this.selectedCategory3 !== "00000000-0000-0000-0000-000000000000") {
                            this.setCat3();
                        }
                    }
                }
                if (this.selectedProject && this.selectedProject !== "00000000-0000-0000-0000-000000000000") {
                    this.selectedProjectId = this.selectedProject;
                }
            } else {
            this.dateTimeInput1 = new Date(currentDate);
            this.dateTimeInput2 = new Date(this.dateTimeInput1);
            this.dateTimeInput2.setHours(this.dateTimeInput2.getHours() + 1);
            }

            this.openModalProgrammatically();
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
            if (currentDayOfWeek == 0) {
                currentDayOfWeek = 7
            };

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
            localDate.setMinutes(localDate.getMinutes() - offsetMinutes);
            this.dateTimeInput1 = localDate.toISOString().slice(0, 16);
        },
        updateDateTimeInput2(event) {
            const localDate = new Date(event.target.value);
            const offsetMinutes = localDate.getTimezoneOffset();
            localDate.setMinutes(localDate.getMinutes() - offsetMinutes);
            this.dateTimeInput2 = localDate.toISOString().slice(0, 16);
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
            this.curday = this.selectedDate.getDate();
            this.handleCellClick(this.curday);
        },
        toggleHover() {
            this.hovered = !this.hovered;
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
        },
        async getWeekReports() {
            let date = new Date(this.cursorDate);

            // Знаходимо перший день тижня (понеділок)
            let firstDayOfWeek = new Date(date);
            firstDayOfWeek.setDate(date.getDate() - date.getDay() + (date.getDay() === 0 ? -6 : 1));

            // Знаходимо останній день тижня (п'ятниця)
            let lastDayOfWeek = new Date(firstDayOfWeek);
            lastDayOfWeek.setDate(firstDayOfWeek.getDate() + 6);

            // Встановлюємо час для першого дня на 9:00:00
            firstDayOfWeek.setHours(0, 0, 0, 0);

            // Встановлюємо час для останнього дня на 18:00:00
            lastDayOfWeek.setHours(23, 59, 59, 999);

            let responseReports = await axios.get("/Report/GetReports", {
                params: {
                    idCu: this.selectedCustomerId,
                    idCo: this.selectedCompanyId,
                    idEm: this.selectedEmployeeId,
                    datestart: firstDayOfWeek,
                    dateend: lastDayOfWeek,
                }
            });

            this.reportsWeek = responseReports.data;
            console.log(this.reportsWeek);
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
