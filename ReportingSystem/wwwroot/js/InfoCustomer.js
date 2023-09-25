new Vue({
    el: '#InfoCustomer',
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
        GeneratePassword() {
            const length = 8;
            const charset = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"; // Можна додати інші символи за потреби
            let password = "";

            for (let i = 0; i < length; i++) {
                const randomIndex = Math.floor(Math.random() * charset.length);
                password += charset[randomIndex];
            }

            this.personalInfo.password = password;
            this.$forceUpdate();
        },
        async Init() {
            console.log(this.rol);
            this.personalInfo = await this.getCustomer();
            console.log(personalInfo);
            this.personalInfo.endTimeLicense = this.formatDate(this.personalInfo.endTimeLicense);
            this.personalInfo.dateRegistration = this.formatDate(this.personalInfo.dateRegistration);
        },
        toggleModal(type) {

            this.modalType = type;
            this.indexEmployee = index;

            if (type === 2) {
                this.modalEmployeeActive = false;
                this.modalOperation = 'Ви впевнені, що хочете редагувати дані ?';
                this.modalTitle = 'Редагування власних даних';
            }
        },
        async editCustomer() {
            try {
                const response = await axios.post('/Customers/EditCustomer', this.personalInfo);
            } catch (error) {
                console.error('Помилка під час виклику методу EditCustomer:', error);
            }
        },

        async getCustomer() {
            let response = await axios.get("/Customers/GetCustomer", {
                params: {
                    idCu: this.selectedCustomerId,
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