new Vue({
    el: '#IndexInfo',
    data: {
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

            this.personalInfo = await this.getEmployee();
            console.log(this.personalInfo);
            this.personalInfo.birthDate = this.formatDate(this.personalInfo.birthDate);
            this.personalInfo.workStartDate = this.formatDate(this.personalInfo.workStartDate);

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