new Vue({
    el: '#Companies',
    data: {
        customerId: '',
        companyId: '',
        employeeId: '',
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
    },
    mounted() {
        this.Init();
    },
    computed: {
        countFilteredCompanies() {
            const nameFilter = this.searchQuery ? this.searchQuery.toLowerCase() : '';

            let filteredList = this.companies.filter((company) => {
                const nameMatches = !nameFilter || company.name.toLowerCase().includes(nameFilter);
                const isInArchive = company.status.companyStatusType === 3;

                if (this.showArchive) {
                    return isInArchive && nameMatches;
                } else {
                    return !isInArchive && nameMatches;
                }
            });

            return filteredList.length;
        },
        filteredCompanies() {
            const nameFilter = this.searchQuery ? this.searchQuery.toLowerCase() : '';

            let filteredList = this.companies.filter((company) => {
                const nameMatches = !nameFilter || company.name.toLowerCase().includes(nameFilter);
                const isInArchive = company.status.companyStatusType === 3;

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
            let response = await axios.get("/Companies/GetActualCompanies");
            this.companies = response.data;
            this.pageCount = Math.ceil(this.countFilteredCompanies / this.itemsPerPage);
            console.log(this.companies);
        },
        generateGuid() {
            const template = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx';
            return template.replace(/[xy]/g, function (c) {
                const r = Math.random() * 16 | 0;
                const v = c === 'x' ? r : (r & 0x3 | 0x8);
                return v.toString(16);
            });
        },
        async checkCompany() {
            const guid = this.generateGuid();
            const edrpu = this.editEDRPU;
            const ar = [guid, edrpu];
            console.log(ar);
            await axios.post('/Companies/PostCheckCompany', ar);
            let response = await axios.get('/Companies/GetCheckCompany', {
                params: {
                    id: guid
                }
            });
            console.log(response.data);
            this.newCompany = response.data;
            console.log("rd " + this.newCompany);
            console.log("rd " + this.newCompany.name);
            if (this.newCompany.name !== undefined) {
                this.useOpenDataBot = false;
                this.editCompanyName = this.newCompany.name;
                this.editEDRPU = this.newCompany.code;
                this.editCompanyAddress = this.newCompany.address;
                this.editCompanyActions = this.newCompany.actions;
                this.editCompanyPhone = this.newCompany.phone;
                this.editCompanyEmail = this.newCompany.email;
            } else {
                this.modalOperation = "Не вдалося знайти компанію за даним кодом";
            };
        },
        setItemsPerPage(count) {
            this.itemsPerPage = count;
            this.pageCount = Math.ceil(this.countFilteredCompanies / this.itemsPerPage);
        },
        nextBatch() {
            this.pageCount = Math.ceil(this.countFilteredCompanies / this.itemsPerPage);

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
        async confirmEditCompany() {
            const v0 = this.filteredCompanies[this.indexCompany].id;
            const v1 = this.editCompanyName;
            const v2 = this.editCompanyAddress;
            const v3 = this.editCompanyActions;
            const v4 = this.editCompanyPhone;
            const v5 = this.editCompanyEmail;
            const v6 = this.customerId;
            var ar = [v0, v1, v2, v3, v4, v5, v6];

            try {
                await axios.post('/Companies/EditCompany', ar);
            } catch (error) {
                console.error('Помилка під час виклику методу EditCompany:', error);
            }

            this.Init();
            this.closeAllAccordions();
        },
        async confirmArchiveCompany() {
            const v0 = this.filteredCompanies[this.indexCompany].id;
            const v1 = this.customerId;
            var ar = [v0, v1];

            try {
                await axios.post('/Companies/ArchiveCompany', ar);
            } catch (error) {
                console.error('Помилка під час виклику методу ArchiveCompany:', error);
            }

            this.Init();
            this.closeAllAccordions();
        },
        async confirmDeleteCompany() {
            const v0 = this.filteredCompanies[this.indexCompany].id;
            const v1 = this.customerId;
            var ar = [v0, v1];

            try {
                await axios.post('/Companies/DeleteCompany', ar);
            } catch (error) {
                console.error('Помилка під час виклику методу DeleteCompany:', error);
            }

            this.Init();
            this.closeAllAccordions();
        },

        async confirmCreateCompany() {
            const v0 = this.editCompanyName;
            const v1 = this.editEDRPU;
            const v2 = this.editCompanyAddress;
            const v3 = this.editCompanyActions;
            const v4 = this.editCompanyPhone;
            const v5 = this.editCompanyEmail;
            const v6 = this.customerId;

            var ar = [v0, v1, v2, v3, v4, v5, v6];
            console.log('create');
            try {
                await axios.post('/Companies/CreateCompany', ar);
            } catch (error) {
                console.error('Помилка під час виклику методу DeleteCompany:', error);
            }
            this.Init();
            this.closeAllAccordions();
        },

        toggleModal(type, index) {

            this.modalType = type;
            this.indexCompany = index;

            if (type === 1) {
                this.modalCompanyActive = false;
                this.modalOperation = 'Ви впевнені, що хочете створити компанію? ' + this.modalName;
                this.modalTitle = 'Створення нової компанії';
                this.editCompanyName = this.modalName;
            }
            if (type === 2) {
                this.editCompanyName = this.filteredCompanies[index].name;
                this.editCompanyAddress = this.filteredCompanies[index].address;
                this.editCompanyActions = this.filteredCompanies[index].actions;
                this.editCompanyPhone = this.filteredCompanies[index].phone;
                this.editCompanyEmail = this.filteredCompanies[index].email;

                this.modalName = this.filteredCompanies[index].name;
                this.modalCompanyActive = false;
                this.modalOperation = 'Ви впевнені, що хочете редагувати компанію? ' + this.modalName;
                this.modalTitle = 'Редагування компанії';
                this.editCompanyName = this.modalName;
            }
            if (type === 3) {
                this.modalName = this.filteredCompanies[index].name;
                if (!this.modalCompanyActive) {
                    this.modalOperation = 'Ви впевнені, що хочете архівувати компанію? ' + this.modalName;
                }
                this.modalTitle = 'Архівування компанії';
            }
            if (type === 4) {
                this.modalName = this.filteredCompanies[index].name;
                if (!this.modalCompanyActive) {
                    this.modalOperation = 'Ви впевнені, що хочете видалити компанію? ' + this.modalName;
                }
                this.modalTitle = 'Видалення компанії';
            }
        },
        closeAllAccordions() {
            this.pageCount = Math.ceil(this.countFilteredCompanies / this.itemsPerPage);
            const accordionItems = document.querySelectorAll(".accordion-collapse");
            accordionItems.forEach(item => {
                item.classList.remove("show");
            });
        },
    },
});
