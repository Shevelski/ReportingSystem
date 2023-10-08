
new Vue({
    el: '#ProjectsCategories',
    data: {
        searchQueryCategory:'',
        pageCount:1,
        pageCur:1,
        itemsPerPage: 10,
        modalType: 0,
        modalIndex: null,
        modalName: '',
        modalOperation: null,
        modalTitle: null,
        modalCategoryUsed:false,
        editCategoryName:'',
        navigLevel0: -1,
        navigLevel1: -1,
        navigLevel2: -1,
        navigLevel3: -1,
        levelCategory:0,
        categoriesLevel1: [0],
        customerId: '',
        companyId: '',
        employeeId: '',
        rol: '',
        selectedCompanyId: 0,
        selectedCustomerId: 0,
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
        countFilteredCategory(){
            const nameFilter = this.searchQueryCategory ? this.searchQueryCategory.toLowerCase() : '';
               
            let filteredList = this.categoriesLevel1.filter((categoryLevel1) => {
                const nameMatches = !nameFilter || categoryLevel1.name.toLowerCase().includes(nameFilter);
                return nameMatches;
            });
            return filteredList.length;
        },
        filteredCategory(){
            const nameFilter = this.searchQueryCategory ? this.searchQueryCategory.toLowerCase() : '';
               
            let filteredList = this.categoriesLevel1.filter((categoryLevel1) => {
            const nameMatches = !nameFilter || categoryLevel1.name.toLowerCase().includes(nameFilter);
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
            }
            if (this.rol == 'Customer') {
                this.selectedCustomerId = this.customerId;
                await this.updateCompanies();
            }
            if (this.rol == 'CEO') {
                this.selectedCustomerId = this.customerId;
                this.selectedCompanyId = this.companyId;
            }
            if (this.rol == 'User') {
                this.selectedCustomerId = this.customerId;
                this.selectedCompanyId = this.companyId;
            }

            let response = await axios.get("/ProjectsCategories/GetCategories", {
                params: {
                    idCu: this.selectedCustomerId,
                    idCo: this.selectedCompanyId
                }
            });
            
            this.categoriesLevel1 = response.data;

            this.pageCount = Math.ceil(this.countFilteredCategory / this.itemsPerPage);
        },
        setItemsPerPage(count) {
            this.itemsPerPage = count;
        },
        nextBatch() {
            this.pageCount = Math.ceil(this.countFilteredCategory / this.itemsPerPage);

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
        async confirmCreateCategory() {

            var navigLevel = this.navigLevel0 + this.itemsPerPage * this.pageCur - this.itemsPerPage;

            const v0 = this.selectedCustomerId;
            const v1 = this.selectedCompanyId;
            var v2 = null;
            var v3 = null;
            var v4 = null;
            var v5 = null;


            if (navigLevel != -1) {
                v2 = this.filteredCategory[this.navigLevel0].id;
            }
            if (this.navigLevel1 != -1) {
                v3 = this.filteredCategory[this.navigLevel0].categoriesLevel1[this.navigLevel1].id;
            }
            if (this.navigLevel2 != -1) {
                v4 = this.filteredCategory[this.navigLevel0].categoriesLevel1[this.navigLevel1].categoriesLevel2[this.navigLevel2].id;
            }
            if (this.navigLevel3 != -1) {
                v5 = this.filteredCategory[this.navigLevel0].categoriesLevel1[this.navigLevel1].categoriesLevel2[this.navigLevel2].categoriesLevel3[this.navigLevel3].id;
            }
            
            const v6 = this.editCategoryName;
            
            const ar = [v0, v1, v2, v3, v4, v5, v6];

            try {
                await axios.post('/ProjectsCategories/CreateCategory', ar);
            } catch (error) {
                console.error('Помилка під час виклику методу CreateCategory:', error);
            }

            this.Init();

            this.editCategoryName = '';
        },
        async confirmEditCategory() {

            var navigLevel = this.navigLevel0 + this.itemsPerPage * this.pageCur - this.itemsPerPage;

            const v0 = this.selectedCustomerId;
            const v1 = this.selectedCompanyId;
            var v2 = null;
            var v3 = null;
            var v4 = null;
            var v5 = null;


            if (navigLevel != -1) {
                v2 = this.filteredCategory[this.navigLevel0].id;
            }
            if (this.navigLevel1 != -1) {
                v3 = this.filteredCategory[this.navigLevel0].categoriesLevel1[this.navigLevel1].id;
            }
            if (this.navigLevel2 != -1) {
                v4 = this.filteredCategory[this.navigLevel0].categoriesLevel1[this.navigLevel1].categoriesLevel2[this.navigLevel2].id;
            }
            if (this.navigLevel3 != -1) {
                v5 = this.filteredCategory[this.navigLevel0].categoriesLevel1[this.navigLevel1].categoriesLevel2[this.navigLevel2].categoriesLevel3[this.navigLevel3].id;
            }

            const v6 = this.editCategoryName;

            const ar = [v0, v1, v2, v3, v4, v5, v6];

            try {
                await axios.post('/ProjectsCategories/EditNameCategory', ar);
            } catch (error) {
                console.error('Помилка під час виклику методу EditNameCategory:', error);
            }

            this.Init();

            this.editCategoryName = '';

         },

        async confirmDeleteCategory() {

            var navigLevel = this.navigLevel0 + this.itemsPerPage * this.pageCur - this.itemsPerPage;

            const v0 = this.selectedCustomerId;
            const v1 = this.selectedCompanyId;

            if (!this.modalCategoryUsed) { 
                
                var v2 = null;
                var v3 = null;
                var v4 = null;
                var v5 = null;

                if (navigLevel != -1) {
                    v2 = this.filteredCategory[this.navigLevel0].id;
                }
                if (this.navigLevel1 != -1) {
                    v3 = this.filteredCategory[this.navigLevel0].categoriesLevel1[this.navigLevel1].id;
                }
                if (this.navigLevel2 != -1) {
                    v4 = this.filteredCategory[this.navigLevel0].categoriesLevel1[this.navigLevel1].categoriesLevel2[this.navigLevel2].id;
                }
                if (this.navigLevel3 != -1) {
                    v5 = this.filteredCategory[this.navigLevel0].categoriesLevel1[this.navigLevel1].categoriesLevel2[this.navigLevel2].categoriesLevel3[this.navigLevel3].id;
                }

                const ar = [v0, v1, v2, v3, v4, v5];

                try {
                    await axios.post('/ProjectsCategories/DeleteCategory', ar)
                } catch (error) {
                    console.error('Помилка під час виклику методу DeleteCategory:', error);
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
                this.modalName = this.filteredCategory[indexLevel0].name;
            }
            if (indexLevel1 != -1) {
                this.modalName = this.filteredCategory[indexLevel0].categoriesLevel1[indexLevel1].name;
                this.modalCategoryUsed = this.filteredCategory[indexLevel0].categoriesLevel1[indexLevel1].projects.length !== 0;
                this.projectsUsed = this.filteredCategory[indexLevel0].categoriesLevel1[indexLevel1].projects;
            }
            if (indexLevel2 != -1) {
                this.modalName = this.filteredCategory[indexLevel0].categoriesLevel1[indexLevel1].categoriesLevel2[indexLevel2].name;
                this.modalCategoryUsed = this.filteredCategory[indexLevel0].categoriesLevel1[indexLevel1].categoriesLevel2[indexLevel2].projects.length !== 0;
                this.projectsUsed = this.filteredCategory[indexLevel0].categoriesLevel1[indexLevel1].categoriesLevel2[indexLevel2].projects;

            }
            if (indexLevel3 != -1) {
                this.modalName = this.filteredCategory[indexLevel0].categoriesLevel1[indexLevel1].categoriesLevel2[indexLevel2].categoriesLevel3[indexLevel3].name;
                this.modalCategoryUsed = this.filteredCategory[indexLevel0].categoriesLevel1[indexLevel1].categoriesLevel2[indexLevel2].categoriesLevel3[indexLevel3].projects.length !== 0;
                this.projectsUsed = this.filteredCategory[indexLevel0].categoriesLevel1[indexLevel1].categoriesLevel2[indexLevel2].categoriesLevel3[indexLevel3].projects;
            }

            if (type === 1) {
                this.modalCategoryUsed = false;
                this.modalOperation = 'Ви впевнені, що хочете додати категорію в структуру? ' ;
                this.modalTitle = 'Додавання категорії';
            }
            if (type === 2) {
                this.modalCategoryUsed = false;
                this.modalOperation = 'Ви впевнені, що хочете редагувати категорію проекту - ' + this.modalName +  ' ? ';
                this.modalTitle = 'Редагування категорії';
                this.editCategoryName = this.modalName;
            }
            if (type === 3) {
                if (!this.modalCategoryUsed) {
                    this.modalOperation = 'Ви впевнені, що хочете видалити категорію проекту - ' + this.modalName + ' ? ';
                } else {
                    this.modalOperation = 'Категорія з назвою ' + this.modalName + ' використовується в проектах:\n' + this.projectsUsed.join('\n') + '. Видалення не буде виконане';;
                }
                this.modalTitle = 'Видалення категорії';
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
