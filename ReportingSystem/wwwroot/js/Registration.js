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
            const v0 = "email";
            const v1 = "firstName";
            const v2 = "secondName";
            const v3 = "thirdName";
            const v4 = "phone";
            const v5 = "password";
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