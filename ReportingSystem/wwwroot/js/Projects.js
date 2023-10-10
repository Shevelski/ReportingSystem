
new Vue({
    el: '#Projects',
    data: {
        newProject: {
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
        projectId: '',
        rol:'',
        beforeEditProject: '',
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
        showProjectInfo: false,
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
        indexProject: 0,
        modalName: '',
        modalOperation: null,
        modalTitle: null,
        modalProjectActive: false,
        editProjectFirstName: '',
        editProjectSecondName: '',
        editProjectThirdName: '',
        newProject: {},
        projects: [0],
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
        
        countFilteredProjects() {
            const nameFilter = this.searchQuery ? this.searchQuery.toLowerCase() : '';

            let filteredList = this.projects.filter((project) => {
                const nameMatches = !nameFilter || project.name.toLowerCase().includes(nameFilter);
                const isInArchive = project.status && project.status.projectStatusType == 2;

                if (this.showArchive) {
                    return isInArchive && nameMatches;
                } else {
                    return !isInArchive && nameMatches;
                }
            });

            return filteredList.length;
        },

        filteredProjects() {
            const nameFilter = this.searchQuery ? this.searchQuery.toLowerCase() : '';

            let filteredList = this.projects.filter((project) => {
                const nameMatches = !nameFilter || project.name.toLowerCase().includes(nameFilter);
                const isInArchive = project.status && project.status.projectStatusType == 2;

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
            if (this.rol == 'ProjectManager') {
                this.selectedCustomerId = this.customerId;
                this.selectedCompanyId = this.companyId;
            }

            this.projects = await this.getProjects();

            console.log(this.projects);

            //for (let i = 0; i < this.projects.length; i++) {
            //    this.projects[i].birthDate = this.dateCSharpToJs(this.projects[i].birthDate);
            //    this.projects[i].workStartDate = this.dateCSharpToJs(this.projects[i].workStartDate);
            //    this.projects[i].workEndDate = this.dateCSharpToJs(this.projects[i].workEndDate);
            //};

            this.pageCount = Math.ceil(this.countFilteredProjects / this.itemsPerPage);
        },

        
        
        async getProjects() {
            let response = await axios.get("/Projects/GetProjects", {
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
        isFormEmpty() {
            const project = this.newProject;
            return (
                project.firstName == '' || project.firstName == null || project.firstName == undefined ||
                project.secondName == '' || project.secondName == null || project.secondName == undefined ||
                project.thirdName == '' || project.thirdName == null || project.thirdName == undefined ||
                project.addSalary == '' || project.addSalary == null || project.addSalary == undefined
            )
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
        ToogleMode(mode) {
            if (this.mode == 'edit') {
                this.toggleModal(2, this.indexProject);
            } else {
                this.beforeEditProject = this.filteredProjects[this.indexProject];
            }

            if (this.mode != mode) {
                this.mode = mode;
            }
        },
        async editProject() {
            this.ToogleMode('standart');
            try {
                const response = await axios.post('/Projects/EditProject', this.filteredProjects[this.indexProject]);
            } catch (error) {
                console.error('Помилка під час виклику методу EditProject:', error);
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
        setIndexProject(index) {
            if (index == this.indexProject && this.showProjectInfo) {
                this.showProjectInfo = false;
            } else {
                this.showProjectInfo = true;
                this.indexProject = index;
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
                this.newProject.password = password;
                this.$forceUpdate();
            } else {
                this.filteredProjects[this.indexProject].password = password;
                
            }
            
        },
        shortNameProject(index) {

            var formattedName = this.filteredProjects[index].secondName + ' ';
            if (this.filteredProjects[index].firstName) {
                formattedName += this.filteredProjects[index].firstName.charAt(0) + '.';
            }
            if (this.filteredProjects[index].thirdName) {
                formattedName += this.filteredProjects[index].thirdName.charAt(0) + '.';
            }
            return formattedName;
        },
        holidayDaysCount(indexProject) {
            const currentYear = new Date().getFullYear();
            if (this.projects[indexProject].holidayDate !== null) {
                const datesThisYear = this.projects[indexProject].holidayDate.filter(date => {
                    return new Date(date).getFullYear() === currentYear;
                });
                return datesThisYear.length;
            } else {
                return 0;
            }   
        },
        hospitalDaysCount(indexProject) {
            const currentYear = new Date().getFullYear();
            if (this.projects[indexProject].hospitalDate !== null) {
                const datesThisYear = this.projects[indexProject].hospitalDate.filter(date => {
                    return new Date(date).getFullYear() === currentYear;
                });
                return datesThisYear.length;
            } else {
                return 0;
            }
        },
        assignmentDaysCount(indexProject) {
            const currentYear = new Date().getFullYear();
            if (this.projects[indexProject].assignmentDate !== null) {
                const datesThisYear = this.projects[indexProject].assignmentDate.filter(date => {
                    return new Date(date).getFullYear() === currentYear;
                });
                return datesThisYear.length;
            } else {
                return 0;
            }
        },
        taketimeoffDaysCount(indexProject) {
            const currentYear = new Date().getFullYear();
            if (this.projects[indexProject].taketimeoffDate !== null) {
                const datesThisYear = this.projects[indexProject].taketimeoffDate.filter(date => {
                    return new Date(date).getFullYear() === currentYear;
                });
                return datesThisYear.length;
            } else {
                return 0;
            }
        },
        setItemsPerPage(count) {
            this.itemsPerPage = count;
            this.pageCount = Math.ceil(this.countFilteredProjects / this.itemsPerPage);
        },
        nextBatch(){
            this.pageCount = Math.ceil(this.countFilteredProjects / this.itemsPerPage);

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
        hasArchiveProject() {
            for (let i = 0; i < this.projects.length; i++) {
                if (this.projects[i].status.projectStatusType === 2) {
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

        async confirmArchiveProject() {
            const idCu = this.selectedCustomerId;
            const idCo = this.selectedCompanyId;
            const idEm = this.filteredProjects[this.indexProject].id;

            const ar = [idCu, idCo, idEm];

            try {
                await axios.post('/Projects/ArchiveProject', ar);
            } catch (error) {
                console.error('Помилка під час виклику методу ArchiveProject:', error);
            }

            this.Init();
            this.closeAllAccordions();
            this.showProjectInfo = !this.showProjectInfo;
        },
        async fromArchiveProject() {
            const idCu = this.selectedCustomerId;
            const idCo = this.selectedCompanyId;
            const idEm = this.filteredProjects[this.indexProject].id;
            const ar = [idCu, idCo, idEm];

            try {
                await axios.post('/Projects/FromArchiveProject', ar);
            } catch (error) {
                console.error('Помилка під час виклику методу FromArchiveProject:', error);
            }
            this.Init();
            this.closeAllAccordions();
            this.showProjectInfo = !this.showProjectInfo;
        },
        async confirmDeleteProject() {
            const idCu = this.selectedCustomerId;
            const idCo = this.selectedCompanyId;
            const idEm = this.filteredProjects[this.indexProject].id;
            const ar = [idCu, idCo, idEm];

            try {
                await axios.post('/Projects/DeleteProject', ar);
            } catch (error) {
                console.error('Помилка під час виклику методу DeleteProject:', error);
            }

            this.Init();
        },    
        async addProject() {
            const v0 = this.selectedCustomerId;
            const v1 = this.selectedCompanyId;
            const v2 = this.newProject.firstName;
            const v3 = this.newProject.secondName;
            const v4 = this.newProject.thirdName;
            const v5 = this.newProject.birthDate;
            const v6 = this.newProject.workStartDate;
            const v7 = this.newProject.namePosition;
            const v8 = this.newProject.login;
            const v9 = this.newProject.rol.rolName;
            const v10 = this.newProject.rol.rolType.toString();
            const v11 = this.newProject.password;

            const ar = [v0, v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11];

            console.log(ar);
            try {
                await axios.post('/Projects/CreateProject', ar);
            } catch (error) {
                console.error('Помилка під час виклику методу CreateProject:', error);
            }
            this.closeAllAccordions();
            this.Init();

        },
        

        toggleModal(type, index) {

            this.modalType = type;
            this.indexProject = index;

            if (type === 1) {
                this.modalProjectActive = false;
                this.modalOperation = ''
                this.modalTitle = 'Додавання співробітника';
                this.editProjectName = this.modalName;
            }

            if (type === 10) {
                this.modalProjectActive = false;
                this.modalOperation = 'Ви впевнені, що хочете додати співробітника? ' + this.modalName;
                this.modalTitle = 'Додавання співробітника';
                this.editProjectName = this.modalName;
            }

            if (type === 2) {
                this.modalProjectActive = false;
                this.modalOperation = 'Ви впевнені, що хочете редагувати співробітника ' + this.beforeEditProject.firstName + " " + this.beforeEditProject.secondName + " " + this.beforeEditProject.thirdName + " ?";
                this.modalTitle = 'Редагування співробітника';
            }
            if (type === 3) {
                if (!this.modalProjectActive) {
                    this.modalOperation = 'Ви впевнені, що хочете архівувати співробітника ' + this.filteredProjects[index].firstName + " " + this.filteredProjects[index].secondName + " " + this.filteredProjects[index].thirdName + " ?";
                }
                this.modalTitle = 'Архівування співробітника';
            }
            if (type === 4) {
                if (!this.modalProjectActive) {
                    this.modalOperation = 'Ви впевнені, що хочете видалити співробітника ' + this.filteredProjects[index].firstName + " " + this.filteredProjects[index].secondName + " " + this.filteredProjects[index].thirdName + " ?";
                }
                this.modalTitle = 'Видалення співробітника';
            }
            if (type === 5) {
                if (!this.modalProjectActive) {
                    this.modalOperation = 'Ви впевнені, що хочете відновити співробітника ' + this.filteredProjects[index].firstName + " " + this.filteredProjects[index].secondName + " " + this.filteredProjects[index].thirdName + " ?";
                }
                this.modalTitle = 'Відновлення співробітника';
            }
        },
        closeAllAccordions() {
            this.pageCount = Math.ceil(this.countFilteredProjects / this.itemsPerPage);
            const accordionItems = document.querySelectorAll(".accordion-collapse");
            accordionItems.forEach(item => {
                item.classList.remove("show");
            });
        },
    },
});
