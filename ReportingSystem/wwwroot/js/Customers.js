new Vue({
    el: '#Customers',
    data: {
        pageCount: 1,
        testEmail: '',
        pageCur: 1,
        itemsPerPage: 10,
        showArchive: false,
        selectedOption: "period",
        openAccordions: [],
        nameFilter: '',
        modalIndex: null,
        modalName: '',
        modalOperation: null,
        modalTitle: null,
        curDate: new Date(),
        showExpired: false,
        modalType: null,
        searchQuery: '',
        historyCount: 0,
        historyArr: [],
        customers: [0],
        bufferCost:
        {
            nextDate: new Date(),
            nextDateCalendar: new Date(),
            subPeriod: '',
            amountBaseCost: null,
            amountCostDown: null,
            allCostDown: null,
            amountGeneralCost: null,
            renewalPeriod: 1,
            renewalUnit: 'month',
            inactiveUser: false,
        },
    },
    mounted() {
        this.Init();
    },
    computed: {
        countFilteredCustomers() {
            const currentDate = new Date();
            const nameFilter = this.searchQuery ? this.searchQuery.toLowerCase() : '';

            let filteredList = this.customers.filter((customer) => {
                const isExpired = new Date(customer.endTimeLicense) < currentDate;
                const nameMatches = !nameFilter || customer.firstName.toLowerCase().includes(nameFilter) || customer.secondName.toLowerCase().includes(nameFilter) || customer.thirdName.toLowerCase().includes(nameFilter) || customer.email.toLowerCase().includes(nameFilter);
                const isInArchive = customer.statusLicence.licenceType === 4;
               
                if (this.showArchive) {
                    return isInArchive && nameMatches;
                } else {
                    return !isInArchive && ((isExpired && this.showExpired && nameMatches) || (!this.showExpired && nameMatches));
                }
            });
            return filteredList.length;
        },
        filteredCustomers() {
            const currentDate = new Date();
            const nameFilter = this.searchQuery ? this.searchQuery.toLowerCase() : '';

            let filteredList = this.customers.filter((customer) => {
                const isExpired = new Date(customer.endTimeLicense) < currentDate;
                const nameMatches = !nameFilter || customer.firstName.toLowerCase().includes(nameFilter) || customer.secondName.toLowerCase().includes(nameFilter) || customer.thirdName.toLowerCase().includes(nameFilter) || customer.email.toLowerCase().includes(nameFilter);
                 
                const isInArchive = customer.statusLicence && customer.statusLicence.licenceType === 4;

                if (this.showArchive) {
                    return isInArchive && nameMatches;
                } else {
                    return !isInArchive && ((isExpired && this.showExpired && nameMatches) || (!this.showExpired && nameMatches));
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
            let response = await axios.get("/Customers/GetCustomers");
            this.customers = response.data;
            console.log(this.customers);
            this.pageCount = Math.ceil(this.countFilteredCustomers / this.itemsPerPage);
            this.defaultPeriod();
            

        },
        setItemsPerPage(count) {
            this.itemsPerPage = count;
            this.pageCount = Math.ceil(this.countFilteredCustomers / this.itemsPerPage);
        },
        nextBatch() {
            this.pageCount = Math.ceil(this.countFilteredCustomers / this.itemsPerPage);
            if (this.pageCur < this.pageCount) {
                this.pageCur++;
            }
            this.closeAllAccordions();
        },
        amountCompany(index) {
            if (Array.isArray(this.filteredCustomers) && this.filteredCustomers[index].companies) {
                return this.filteredCustomers[index].companies.length;
            }
            return 0;
        },
        getTotalEmployeesForUser(userIndex) {
            if (this.filteredCustomers[userIndex] && this.filteredCustomers[userIndex].companies) {
                const companiesOfUser = this.filteredCustomers[userIndex].companies;

                const totalEmployees = companiesOfUser.reduce((total, company) => {
                    if (company.employees) {
                        return total + company.employees.length;
                    }
                    return total;
                }, 0);

                return totalEmployees;
            }
            return 0;
        },
        prevBatch() {
            if (this.pageCur > 1) {
                this.pageCur--;
                this.closeAllAccordions();
            }
        },
        firstBatch() {
            if (this.pageCur !== 1) {
                this.pageCur = 1;
            }
            this.closeAllAccordions();
        },
        isLicenseExpired(company) {
            const statusDate = new Date(company.endTimeLicense);
            return statusDate < this.curDate && !this.showArchive;
        },
        isShowCheckMode(index) {
            const date1 = new Date(this.filteredCustomers[index].endTimeLicense);
            const date2 = new Date(this.curDate);
            return date1 < date2;
        },
        handleRenewalCalendar(index) {
            this.calculateCost(index);
        },
        handleRenewalPeriodChange(index) {
            this.calculateCost(index);
        },
        handleRenewalUnitChange(index) {
            this.calculateCost(index);
        },
        defaultPeriod() {
            this.bufferCost.renewalPeriod = 1;
            this.bufferCost.renewalUnit = 'month';

            var today = this.bufferCost.nextDate;

            var year = today.getFullYear();
            var month = String(today.getMonth() + 1).padStart(2, '0');
            var day = String(today.getDate()).padStart(2, '0');

            this.bufferCost.nextDateCalendar = year + '-' + month + '-' + day;
        },

        calculateCost(index) {

            var date = new Date(this.filteredCustomers[index].endTimeLicense);
            const number = this.bufferCost.renewalPeriod;
            const period = this.bufferCost.renewalUnit;

            var costDown = 0;
            var koefPeriod = 0;



            if (this.bufferCost.inactiveUser) {
                date = new Date();
            }

            if (this.selectedOption == "period") {

                if (period === "day") {
                    date1 = new Date(date.getTime() + number * 86400000);
                } else if (period === "month") {
                    date1 = new Date((date.getTime() + number * 86400000 * 30));
                } else if (period === "quarter") {
                    date1 = new Date((date.getTime() + number * 86400000 * 30 * 3));
                } else if (period === "week") {
                    date1 = new Date(date.getTime() + number * 86400000 * 7);
                } else if (period === "year") {
                    date1 = new Date(date.getTime() + number * 86400000 * 365);
                }

                var year = date1.getFullYear();
                var month = String(date1.getMonth() + 1).padStart(2, '0');
                var day = String(date1.getDate()).padStart(2, '0');

                this.bufferCost.nextDateCalendar = year + '-' + month + '-' + day;
            } else {
                var date1 = new Date(this.bufferCost.nextDateCalendar);
            }

            var timeDiff = Math.floor(Math.abs(date1 - date));
            var daysCount = Math.floor(timeDiff / (24 * 60 * 60 * 1000));
            var weekCount = Math.floor(timeDiff / (7 * 24 * 60 * 60 * 1000));
            var monthCount = Math.floor(timeDiff / (30 * 24 * 60 * 60 * 1000));
            var quarterCount = Math.floor(timeDiff / (3 * 30 * 24 * 60 * 60 * 1000));
            var yearCount = Math.floor(timeDiff / (365 * 24 * 60 * 60 * 1000));

            var years = Math.floor(daysCount / 365);
            var months = Math.floor((daysCount % 365) / 30);
            var days = daysCount - (years * 365) - (months * 30);

            var tempYear = '';
            var tempMonth = '';
            var tempDay = '';

            if (years > 0) {
                tempYear = years + ' рік ';
            } else {
                tempYear = '';
            }

            if (months > 0) {
                tempMonth = months + ' місяць ';
            } else {
                tempMonth = '';
            }

            if (days > 0) {
                tempDay += days + ' день';
            } else {
                tempDay = '';
            }

            this.bufferCost.subPeriod = tempYear + tempMonth + tempDay;

            if (daysCount < 7) {
                costDown = 1;
            }
            if (daysCount >= 7) {
                costDown = 2;
            }
            if (daysCount > 29) {
                costDown = 5;
            }
            if (daysCount > 364) {
                costDown = 10;
            }

            koefPeriod = (daysCount * 1 / 30);

            this.bufferCost.nextDate = date1;
            this.bufferCost.amountBaseCost = 5000;
            this.bufferCost.amountCostDown = Math.round(this.bufferCost.amountBaseCost * costDown / 100);

            if (daysCount >= 7) {

                this.bufferCost.allCostDown = Math.round(this.bufferCost.amountCostDown * daysCount / 30);
                this.bufferCost.amountGeneralCost = Math.round((this.bufferCost.amountBaseCost * koefPeriod) - this.bufferCost.allCostDown);
            }
            else {
                this.bufferCost.amountCostDown = 0;
                this.bufferCost.allCostDown = 0;
                this.bufferCost.amountGeneralCost = Math.round((this.bufferCost.amountBaseCost * daysCount) / 30);
            }
        },
        closeAllAccordions() {
            this.pageCount = Math.ceil(this.countFilteredCustomers / this.itemsPerPage);
            const accordionItems = document.querySelectorAll(".accordion-collapse");
            accordionItems.forEach(item => {
                item.classList.remove("show");
            })
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
        showFormatDate(dateString) {
            const options = { day: '2-digit', month: '2-digit', year: 'numeric' };
            const jsDate = new Date(dateString);
            return jsDate.toLocaleDateString('en-GB', options);
        },

        toggleRenewalAccordion(index) {
            if (this.isRenewalAccordionOpen(index)) {
                this.closeRenewalAccordion(index);
            } else {
                this.calculateCost(index);
                this.openRenewalAccordion(index);
            };

        },
        isRenewalAccordionOpen(index) {
            return this.openAccordions.includes(`renewal-${index}`);
        },
        openRenewalAccordion(index) {
            this.openAccordions.push(`renewal-${index}`);
        },
        closeRenewalAccordion(index) {
            const position = this.openAccordions.indexOf(`renewal-${index}`);
            if (position > -1) {
                this.openAccordions.splice(position, 1);
            }
        },
        async confirmRenewal(index) {
            function padNumber(number, length) {
                return number.toString().padStart(length, '0');
            }

            var jsDate = this.bufferCost.nextDate;
            const formattedDate = `${jsDate.getFullYear()}-${padNumber(jsDate.getMonth() + 1, 2)}-${padNumber(jsDate.getDate(), 2)}T${padNumber(jsDate.getHours(), 2)}:${padNumber(jsDate.getMinutes(), 2)}:${padNumber(jsDate.getSeconds(), 2)}`;

            var period = "";
            if (this.bufferCost.inactiveUser) {
                period = this.bufferCost.subPeriod.toString() + "(неактивний)";
            }
            else {
                period = this.bufferCost.subPeriod.toString();
            }

            var ar = [this.filteredCustomers[index].id, formattedDate, this.bufferCost.amountGeneralCost.toString(), period];
            try {
                await axios.post('/Customers/RenewalLicence', ar);
            } catch (error) {
                console.error('Помилка під час виклику методу RenewalLicence:', error);
            }

            this.Init();
            this.closeRenewalAccordion(index);
            this.closeAllAccordions();
        },
        async confirmCancellation(index) {
            var today = new Date();
            var tomorrow = new Date(today);
            tomorrow.setDate(today.getDate() + 1);

            const date1 = new Date(this.filteredCustomers[index].endTimeLicense);
            const date2 = new Date(tomorrow);

            if (date1 > date2) {

                function padNumber(number, length) {
                    return number.toString().padStart(length, '0');
                }

                var jsDate = tomorrow;
                const formattedDate = `${jsDate.getFullYear()}-${padNumber(jsDate.getMonth() + 1, 2)}-${padNumber(jsDate.getDate(), 2)}T${padNumber(jsDate.getHours(), 2)}:${padNumber(jsDate.getMinutes(), 2)}:${padNumber(jsDate.getSeconds(), 2)}`;

                var ar = [this.filteredCustomers[index].id, formattedDate];

                try {
                    await axios.post('/Customers/CancellationLicence', ar);
                } catch (error) {
                    console.error('Помилка під час виклику методу CancellationLicence:', error);
                }
                this.Init();
                this.closeAllAccordions();
            }

        },
        async confirmArchiving(index) {
            var ar = [this.filteredCustomers[index].id];
            try {
                await axios.post('/Customers/ArchivingLicence', ar);
            } catch (error) {
                console.error('Помилка під час виклику методу ArchivingLicence:', error);
            }
            this.Init();
        },
        async confirmDelete(index) {
            var ar = [this.filteredCustomers[index].id];
            try {
                await axios.post('/Customers/DeleteLicence', ar);
            } catch (error) {
                console.error('Помилка під час виклику методу DeleteLicence:', error);
            }
            this.Init();
        },
        setFilteredIndex(index) {
            this.historyCount = this.filteredCustomers[index].historyOperations.length;
            this.historyArr = this.filteredCustomers[index].historyOperations;
        },
        async createCustomer() {
            console.log("test " + this.testEmail);
            const ar = [this.testEmail]
            try {
                await axios.post('/Customers/CreateCustomer', ar);
            } catch (error) {
                console.error('Помилка під час виклику методу CreateCustomer:', error);
            }
            this.Init();
        },
        //async createCustomer() {
        //    const v0 = "email";
        //    const v1 = "firstName";
        //    const v2 = "secondName";
        //    const v3 = "thirdName";
        //    const v4 = "phone";
        //    const v5 = "password";
        //    const ar = [v0, v1, v2, v3, v4, v5]
        //    try {
        //        await axios.post('/Customers/RegistrationCustomer', ar);
        //    } catch (error) {
        //        console.error('Помилка під час виклику методу ArchivingLicence:', error);
        //    }
        //    this.Init();
        //},
        toggleModal(type, index) {
            this.modalIndex = index;
            this.modalName = this.filteredCustomers[index].firstName + " " + this.filteredCustomers[index].secondName + " " + this.filteredCustomers[index].thirdName;
            this.modalType = type;

            if (type === 1) {
                this.modalOperation = 'Ви впевнені, що хочете продовжити дію ліцензії для ' + this.modalName;
                this.modalTitle = 'Продовження дії ліцензії';
            }
            if (type === 2) {
                this.modalOperation = 'Ви впевнені, що хочете анулювати ліцензію для користувача ' + this.modalName;
                this.modalTitle = 'Анулювання ліцензії';
            }
            if (type === 3) {
                this.modalOperation = 'Ви впевнені, що хочете архівувати ' + this.modalName;
                this.modalTitle = 'Архівування';
            }
            if (type === 5) {
                this.modalOperation = 'Ви впевнені, що хочете видалити ' + this.modalName;
                this.modalTitle = 'Видалення';
            }
            if (type === 4) {
                this.historyCount = this.filteredCustomers[index].historyOperations.length;
                this.historyArr = this.filteredCustomers[index].historyOperations.slice().reverse();

                this.modalOperation = 'Замовник: ' + this.modalName;;
                this.modalTitle = 'Історія оперування ліцензіями';
            }
        },
       
    },
});
