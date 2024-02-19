new Vue({
    el: "#Registration",
    data: {
        email: '',
        firstName: '',
        secondName: '',
        thirdName: '',
        password: '',
        repassword:'',
        phone: '',
        result: '',
        isValidEmail: true,
        isValidPhone: true,
        passwordEquals: true,
        isUnique: true,
    },
    methods: {
        async createCustomer() {
            const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            this.isValidEmail = emailRegex.test(this.email) && this.email.length > 0;
            this.$refs.emailInput.classList.toggle('is-invalid', !this.isValidEmail);

            const phoneRegex = /^\+[0-9]+$/;
            this.isValidPhone = phoneRegex.test(this.phone) && this.phone.length > 0;
            this.$refs.phoneInput.classList.toggle('is-invalid', !this.isValidPhone);

            this.isUnique = await this.IsEmailUnique(this.email);

            this.passwordEquals = this.password === this.repassword && this.password.length > 0 && this.repassword.length > 0 && this.isUnique;
            this.$refs.repasswordInput.classList.toggle('is-invalid', !this.passwordEquals);

            if (this.isValidEmail && this.isValidPhone && this.passwordEquals && this.isUnique) {
                const v0 = this.email;
                const v1 = this.firstName;
                const v2 = this.secondName;
                const v3 = this.thirdName;
                const v4 = this.phone;
                const v5 = this.password;
                const ar = [v0, v1, v2, v3, v4, v5];
                var x = await axios.post('/Customers/RegistrationCustomer', ar);
                if (x) {
                    this.result = "Реєстрація успішна, можете перейти до авторизації";
                } else {
                    this.result = "Реєстрація невдала, можете звернутися до менеджера";
                };
                this.email = '';
                this.firstName = '';
                this.secondName = '';
                this.thirdName = '';
                this.password = '';
                this.repassword = '';
                this.phone = '';
                this.mess = false;
            }
            
        },
        async IsEmailUnique(email) {
            console.log("start");
            var response = await axios.get("/Employees/IsBusyEmail", {
                params: {
                    email: email,
                }
            });
            console.log(response.data);
            console.log("end");
            return !response.data;
        }
    }
})