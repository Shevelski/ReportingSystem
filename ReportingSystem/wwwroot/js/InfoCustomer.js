new Vue({
    el: '#Info',
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
    },
    mounted() {
        this.customerId = document.getElementById('idCu').textContent;
        this.companyId = document.getElementById('idCo').textContent;
        this.employeeId = document.getElementById('idEm').textContent;
        this.rol = document.getElementById('rol').textContent;
        this.Init();

    },
    methods: {
        ToogleMode(mode) {
            if (this.mode == 'edit') {
                this.editUserInfo(); 
            }
            if (this.mode != mode) {
                this.mode = mode;
            }
        },
        async Init() {
            console.log(this.rol);
            this.personalInfo = await this.getEmployee();
            console.log(this.personalInfo);
            this.personalInfo.birthDate = this.formatDate(this.personalInfo.birthDate);
            this.personalInfo.workStartDate = this.formatDate(this.personalInfo.workStartDate);
            if (this.rol != 'Developer') {
                this.calculateDaysCurYear();
            }
            
        },
        async editUserInfo() {
            try {
                const response = await axios.post('/UserInfo/EditUserInfo', this.personalInfo);
            } catch (error) {
                console.error('Помилка під час виклику методу EditUserInfo:', error);
            }
        },

        async getEmployee() {
            let response = await axios.get("/Employees/GetEmployee", {
                params: {
                    idCu: this.selectedCustomerId,
                    idCo: this.selectedCompanyId,
                    idEm: this.employeeId
                }
            });
            return response.data;
        },

        calculateDaysCurYear() {
            const currentYear = new Date().getFullYear();
            const datesThisYear1 = this.personalInfo.holidayDate.filter(date => {
                return new Date(date).getFullYear() === currentYear;
            });
            this.holidayDays = datesThisYear1.length;

            const datesThisYear2 = this.personalInfo.hospitalDate.filter(date => {
                return new Date(date).getFullYear() === currentYear;
            });
            this.hospitalDays = datesThisYear2.length;

            const datesThisYear3 = this.personalInfo.assignmentDate.filter(date => {
                return new Date(date).getFullYear() === currentYear;
            });
            this.assignmentDays = datesThisYear3.length;

            const datesThisYear4 = this.personalInfo.taketimeoffDate.filter(date => {
                return new Date(date).getFullYear() === currentYear;
            });
            this.taketimeoffDays = datesThisYear4.length;
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
        handleImageChange(event) {
            const file = event.target.files[0];
            if (file) {
                const reader = new FileReader();
                reader.onload = (e) => {
                    this.$nextTick(() => {
                        this.updateImage(e.target.result);
                    });
                };
                reader.readAsDataURL(file);
            }
        },
        updateImage(imageData) {
            const imageElement = this.$refs.imageElement;
            if (imageElement) {
                const prefix = 'data:image/jpeg;base64,';
                const imageDataWithoutPrefix = imageData.replace(prefix, '');
                imageElement.src = imageDataWithoutPrefix;
            }
        }

    }
});