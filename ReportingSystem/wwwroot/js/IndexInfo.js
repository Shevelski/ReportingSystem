new Vue({
    el: '#IndexInfo',
    data: {
        birthdays: [0],
        news: [0],
        mode: 'standart',
        personalInfo: [0],
        lengthArray: 0,
        modalTitle: '',
        dateArray: [],
        holidayDays: 0,
        hospitalDays: 0,
        assignmentDays: 0,
        taketimeoffDays: 0,
        modalEmployeeActive: false,
        modalOperation: null,
        modalType: 0,
        selectedCategory: '',
        categories:''
    },
    mounted() {
        this.customerId = document.getElementById('idCu').textContent;
        this.companyId = document.getElementById('idCo').textContent;
        this.employeeId = document.getElementById('idEm').textContent;
        this.rol = document.getElementById('rol').textContent;
        this.Init();

    },
    methods: {
        async Init() {
            console.log(this.rol);

            console.log(this.customerId);
            console.log(this.companyId);
            console.log(this.employeeId);

            if (this.rol == 'Developer' || this.rol == 'DevAdministrator') {
                this.GetEmployeeDevBirthday();
            }
            if (this.rol == 'Customer') {
                this.GetEmployeeChiefBirthday();
            }
            if (this.rol == 'User' || this.rol == 'CEO') {
                this.GetEmployeeBirthday();
            }

            this.personalInfo = await this.getEmployee();
            this.personalInfo.birthDate = this.formatDate(this.personalInfo.birthDate);
            this.personalInfo.workStartDate = this.formatDate(this.personalInfo.workStartDate);
            this.GetCategoriesNews();
            this.GetNews();
        },
        async GetCategoriesNews() { 
            let response = await axios.get("/News/GetCategoriesNews");
            this.categories = response.data;
            console.log(this.categories);
        },
        async GetNews() { 
            let response = await axios.get("/News/GetNews", {
                params: {
                    category: this.selectedCategory
                }
            });
            this.news = response.data;
            console.log(this.news);
        },
        async GetEmployeeDevBirthday() { 
            let response = await axios.get("/Employees/GetEmployeeDevBirthday");
            this.birthdays = response.data;
            console.log(this.birthdays);
        },
        async GetEmployeeChiefBirthday() { 
            let response = await axios.get("/Employees/GetEmployeeChiefBirthday", {
                params: {
                    idCu: this.customerId
                }
            });
            this.birthdays = response.data;
            console.log(this.birthdays);
        },
        async GetEmployeeBirthday() { 
            let response = await axios.get("/Employees/GetEmployeeBirthday", {
                params: {
                    idCu: this.customerId,
                    idCo: this.companyId
                }
            });
            this.birthdays = response.data;
            console.log(this.birthdays);
        },

        async getEmployee() {
            let response = await axios.get("/Employees/GetEmployee", {
                params: {
                    idCu: this.customerId,
                    idCo: this.companyId,
                    idEm: this.employeeId
                }
            });
            return response.data;
        },

        formatDate(dateTimeStr) {
            const date = new Date(dateTimeStr);
            const year = date.getFullYear();
            const month = this.formatNumber(date.getMonth() + 1);
            const day = this.formatNumber(date.getDate());
            return `${year}-${month}-${day}`;
        },

        formatNumber(value) {
            return value.toString().padStart(2, '0');
        },
    }
});