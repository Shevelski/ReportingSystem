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
        ToogleMode(mode) {
            if (this.mode == 'edit') {
                this.toggleModal(2);
            }
            if (this.mode != mode) {
                this.mode = mode;
            }
        },
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
        async EditEmployeeInfo() {
            if (this.rol == 'Developer' || this.rol == 'DevAdministrator') {
                console.log(this.personalInfo);
                try {
                    const response = await axios.post('/Employees/EditAdministrator', this.personalInfo);
                } catch (error) {
                    console.error('Помилка під час виклику методу EditAdministrator:', error);
                }
            } else {
                try {
                    const response = await axios.post('/Employees/EditEmployee', this.personalInfo);
                } catch (error) {
                    console.error('Помилка під час виклику методу EditEmployee:', error);
                }
            }
            
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

        calculateDaysCurYear() {
            const currentYear = new Date().getFullYear();

            if (this.personalInfo.holidayDate != null) {
                const datesThisYear1 = this.personalInfo.holidayDate.filter(date => {
                    return new Date(date).getFullYear() === currentYear;
                });
                this.holidayDays = datesThisYear1.length;
            } else {
                this.holidayDays = 0;
            }
            
            if (this.personalInfo.hospitalDate != null) {
                const datesThisYear2 = this.personalInfo.hospitalDate.filter(date => {
                    return new Date(date).getFullYear() === currentYear;
                });
                this.hospitalDays = datesThisYear2.length;
            } else {
                this.hospitalDays = 0;
            }
            
            if (this.personalInfo.assignmentDate != null) {
                const datesThisYear3 = this.personalInfo.assignmentDate.filter(date => {
                    return new Date(date).getFullYear() === currentYear;
                });
                this.assignmentDays = datesThisYear3.length;
            } else {
                this.assignmentDays = 0;
            }
            
            if (this.personalInfo.taketimeoffDate != null) {
                const datesThisYear4 = this.personalInfo.taketimeoffDate.filter(date => {
                    return new Date(date).getFullYear() === currentYear;
                });
                this.taketimeoffDays = datesThisYear4.length;
            } else {
                this.taketimeoffDays = 0;
            }
            
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
        toggleModal(type) {

            this.modalType = type;
            //this.indexEmployee = index;

            if (type === 2) {
                this.modalEmployeeActive = false;
                this.modalOperation = 'Ви впевнені, що хочете редагувати дані ?';
                this.modalTitle = 'Редагування власних даних';
            }
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