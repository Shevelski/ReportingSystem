
new Vue({
    el: '#ProjectsSteps',
    data: {
        searchQueryStep:'',
        pageCount:1,
        pageCur:1,
        itemsPerPage: 10,
        modalType: 0,
        modalIndex: null,
        modalName: '',
        modalOperation: null,
        modalTitle: null,
        modalStepUsed:false,
        editStepName:'',
        navigLevel0: -1,
        navigLevel1: -1,
        navigLevel2: -1,
        navigLevel3: -1,
        levelStep: 0,
        steps: [0],
        stepsLevel1: [0],
        customerId: '',
        companyId: '',
        employeeId: '',
        rol: '',
        selectedCompanyId: 0,
        selectedCustomerId: 0,
        selectedProjectId: 0,
        selectedCompanyIdCheck: 0,
        selectedCustomerIdCheck: 0,
        companies: [0],
        customers: [0],
    },
    mounted() {
        this.customerId = document.getElementById('idCu').textContent;
        this.companyId = document.getElementById('idCo').textContent;
        this.employeeId = document.getElementById('idEm').textContent;
        this.rol = document.getElementById('rol').textContent;
        this.Init();
    },
    computed: {
        countFilteredSteps(){
            const nameFilter = this.searchQueryStep ? this.searchQueryStep.toLowerCase() : '';
               
            let filteredList = this.steps.filter((step) => {
                const nameMatches = !nameFilter || step.name.toLowerCase().includes(nameFilter);
                return nameMatches;
            });
            return filteredList.length;
        },
        filteredSteps(){
            const nameFilter = this.searchQueryStep ? this.searchQueryStep.toLowerCase() : '';
               
            let filteredList = this.steps.filter((step) => {
            const nameMatches = !nameFilter || step.name.toLowerCase().includes(nameFilter);
            return nameMatches;
            });

                if (this.pageCur === 1) {
                    return filteredList.slice(0, this.pageCur * this.itemsPerPage);
                } else
                {
                    return filteredList.slice(this.pageCur * this.itemsPerPage - this.itemsPerPage, this.pageCur * this.itemsPerPage);
            }
            },
        },
    methods: {
        async Init() {
            if (this.rol == 'Developer' || this.rol == 'DevAdministrator') {
                await this.updateCustomers();
                await this.updateCompanies();
                await this.updateProjects();
            }
            if (this.rol == 'Customer') {
                this.selectedCustomerId = this.customerId;
                await this.updateCompanies();
                await this.updateProjects();
            }
            if (this.rol == 'CEO') {
                this.selectedCustomerId = this.customerId;
                this.selectedCompanyId = this.companyId;
                await this.updateProjects();
            }
            if (this.rol == 'User') {
                this.selectedCustomerId = this.customerId;
                this.selectedCompanyId = this.companyId;
            }

            let response = await axios.get("/ProjectsSteps/GetStepsProjects", {
                params: {
                    idCu: this.selectedCustomerId,
                    idCo: this.selectedCompanyId,
                    idPr: this.selectedProjectId
                }
            });
            
            this.steps = response.data;
            console.log(this.steps);
            this.pageCount = Math.ceil(this.countFilteredSteps / this.itemsPerPage);
        },
        setItemsPerPage(count) {
            this.itemsPerPage = count;
        },
        nextBatch() {
            this.pageCount = Math.ceil(this.countFilteredSteps / this.itemsPerPage);

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
        showFormatDate(dateString) {
            const options = { day: '2-digit', month: '2-digit', year: 'numeric' };
            const jsDate = new Date(dateString);
            return jsDate.toLocaleDateString('en-GB', options);
        },
        async updateProjects() {
            let responseProjects = '';
            responseProjects = await axios.get("/Projects/GetProjects", {
                params: {
                    idCu: this.selectedCustomerId,
                    idCo: this.selectedCompanyId,
                }
            });
            this.projects = responseProjects.data;
            if (this.selectedProjectId == 0) {
                this.selectedProjectId = this.projects[0].id;
            }
            this.IsNewSelectedProject = false;
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
        async confirmCreateStep() {

            var navigLevel = this.navigLevel0 + this.itemsPerPage * this.pageCur - this.itemsPerPage;

            const v0 = this.selectedCustomerId;
            const v1 = this.selectedCompanyId;
            var v2 = this.selectedProjectId;



            var v3 = this.filteredSteps[this.steps].id;
            
            const v4 = this.editStepName;
            
            const ar = [v0, v1, v2, v3, v4];

            try {
                await axios.post('/ProjectsSteps/CreateStep', ar);
            } catch (error) {
                console.error('Помилка під час виклику методу CreateStep:', error);
            }

            this.Init();

            this.editStepName = '';
        },
        async confirmEditStep() {

            var navigLevel = this.navigLevel0 + this.itemsPerPage * this.pageCur - this.itemsPerPage;

            const v0 = this.selectedCustomerId;
            const v1 = this.selectedCompanyId;
            var v2 = this.selectedProjectId;
            var v3 = this.filteredSteps[this.navigLevel0].id;
            const v4 = this.editStepName;
            const ar = [v0, v1, v2, v3, v4];

            try {
                await axios.post('/ProjectsSteps/EditNameStep', ar);
            } catch (error) {
                console.error('Помилка під час виклику методу EditNameStep:', error);
            }

            this.Init();

            this.editStepName = '';

         },
        setFilteredIndex(index) {
        },
        async confirmDeleteStep() {

            var navigLevel = this.navigLevel0 + this.itemsPerPage * this.pageCur - this.itemsPerPage;

            const v0 = this.selectedCustomerId;
            const v1 = this.selectedCompanyId;
            const v2 = this.selectedProjectId;

            if (!this.modalStepUsed) { 
                
                var v3 = this.filteredSteps[this.navigLevel0].id;
                const ar = [v0, v1, v2, v3];

                try {
                    await axios.post('/ProjectsSteps/DeleteSteps', ar)
                } catch (error) {
                    console.error('Помилка під час виклику методу DeleteSteps:', error);
                }
                this.firstBatch();
                this.Init();
            }
        },

        toggleModal(type, indexLevel0, indexLevel1, indexLevel2, indexLevel3) {

            this.navigLevel0 = indexLevel0;
            this.navigLevel1 = indexLevel1;
            this.navigLevel2 = indexLevel2;
            this.navigLevel3 = indexLevel3;

            this.modalType = type;

            if (indexLevel0 != -1) {
                this.modalName = this.filteredSteps[indexLevel0].name;
            }

            if (type === 1) {
                this.modalStepUsed = false;
                this.modalOperation = 'Ви впевнені, що хочете додати етап до проекту? ' ;
                this.modalTitle = 'Додавання етапу проекту';
            }
            if (type === 2) {
                this.modalStepUsed = false;
                this.modalOperation = 'Ви впевнені, що хочете редагувати етап проекту - ' + this.modalName +  ' ? ';
                this.modalTitle = 'Редагування етапу проекту';
                this.editStepName = this.modalName;
            }
            if (type === 3) {
                if (!this.modalStepUsed) {
                    this.modalOperation = 'Ви впевнені, що хочете видалити етап проекту - ' + this.modalName + ' ? ';
                } else {
                    this.modalOperation = 'Етап з назвою ' + this.modalName + ' використовується в проектах:\n' + this.projectsUsed.join('\n') + '. Видалення не буде виконане';;
                }
                this.modalTitle = 'Видалення етапу';
            }
        },
        closeAllAccordions() {
            const accordionItems = document.querySelectorAll(".accordion-collapse");
            accordionItems.forEach(item => {
                item.classList.remove("show");
            });
        },
    },
});
