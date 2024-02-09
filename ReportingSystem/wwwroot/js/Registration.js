new Vue({
    el: "#Registration",
    data: {
        email: '',
        firstName: '',
        secondName: '',
        thirdName: '',
        password: '',
        phone: '',
        result:'',
    },
    methods: {
        async createCustomer() {
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
            this.phone = '';
        },
    }
})