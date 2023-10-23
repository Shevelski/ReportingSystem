new Vue({
    el: "#Registration",
    data: {
        email: '',
        firstName: '',
        secondName: '',
        thirdName: '',
        password: '',
        phone: '',
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
            try 
            {
                await axios.post('/Customers/RegistrationCustomer', ar);
            } 
            catch (error) 
            {
                console.error('Помилка під час виклику методу RegistrationCustomer:', error);
            }
            },
    }
})