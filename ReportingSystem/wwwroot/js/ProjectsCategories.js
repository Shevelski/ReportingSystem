
new Vue({
    el: '#ProjectsCategories',
    data: {
        searchQueryCategory:'',
        pageCount:1,
        pageCur:1,
        itemsPerPage: 10,
        modalType: 0,
        modalIndex: null,
        modalName: '',
        modalOperation: null,
        modalTitle: null,
        modalCategoryUsed:false,
        editCategoryName:'',
        navigLevel1: -1,
        navigLevel2: -1,
        navigLevel3: -1,
        levelCategory:0,
        categoriesLevel1:[0],
    },
    mounted() {
        this.Init();
    },
    computed: {
        countFilteredCategory(){
            const nameFilter = this.searchQueryCategory ? this.searchQueryCategory.toLowerCase() : '';
               
            let filteredList = this.categoriesLevel1.filter((categoryLevel1) => {
                const nameMatches = !nameFilter || categoryLevel1.name.toLowerCase().includes(nameFilter);
                return nameMatches;
            });
            return filteredList.length;
        },
        filteredCategory(){
            const nameFilter = this.searchQueryCategory ? this.searchQueryCategory.toLowerCase() : '';
               
            let filteredList = this.categoriesLevel1.filter((categoryLevel1) => {
            const nameMatches = !nameFilter || categoryLevel1.name.toLowerCase().includes(nameFilter);
            return nameMatches;
            });

                if (this.pageCur === 1) {
                    return filteredList.slice(0, this.pageCur * this.itemsPerPage);
                } else
                {
                    return filteredList.slice(this.pageCur * this.itemsPerPage - this.itemsPerPage, this.pageCur * this.itemsPerPage);
            }
            },
        },
    methods: {
        async Init() {
            let response = await axios.get("/ProjectCategory/GetCategories");
            this.categoriesLevel1 = response.data;
        },
        setItemsPerPage(count) {
            this.itemsPerPage = count;
        },
        nextBatch(){
            this.pageCount = Math.ceil(this.countFilteredCategory / this.itemsPerPage);

            if (this.pageCur < this.pageCount) {
                this.pageCur++;
            }
        },
        prevBatch() {
            if (this.pageCur > 1) {
                this.pageCur--;
            }
        },
        firstBatch() {
            if (this.pageCur !== 1) {
                this.pageCur = 1;
            }
        },
        async confirmCreateCategory() {

            var navigLevel = this.navigLevel1 + this.itemsPerPage * this.pageCur - this.itemsPerPage;

            var id1 = null;
            if (this.navigLevel1 == -1) {
                navigLevel = -1;
            } else {
                id1 = this.filteredCategory[this.navigLevel1].id;
            }

            const ar = [navigLevel, this.navigLevel2, this.navigLevel3];

            try {
                await axios.post('/ProjectCategory/CreateCategory', ar, {
                    params: { newName: this.editCategoryName, idLevel1: id1 }
                });
            } catch (error) {
                console.error('Помилка під час виклику методу CreateCategory:', error);
            }
            this.Init();

            this.editCategoryName = '';

 
        },
        async confirmEditCategory() {

            const navigLevel = this.navigLevel1 + this.itemsPerPage * this.pageCur - this.itemsPerPage;

            const id1 = this.filteredCategory[this.navigLevel1].id;

            const ar = [navigLevel, this.navigLevel2, this.navigLevel3];

            try {
                await axios.post('/ProjectCategory/EditNameCategory', ar, {
                    params: { newName: this.editCategoryName, idLevel1: id1 } 
                });
            } catch (error) {
                console.error('Помилка під час виклику методу EditNameCategory:', error);
            }

            this.Init();

            this.editCategoryName = '';

         },

        async confirmDeleteCategory() {

            if (!this.modalCategoryUsed) { 
                var navigLevel = this.navigLevel1 + this.itemsPerPage * this.pageCur - this.itemsPerPage;

                var id1 = null;
                var id2 = null;
                var id3 = null;

                if (this.navigLevel1 != -1) id1 = this.filteredCategory[this.navigLevel1].id;
                if (this.navigLevel2 != -1) id2 = this.filteredCategory[this.navigLevel1].categoriesLevel2[this.navigLevel2].id;
                if (this.navigLevel3 != -1) id3 = this.filteredCategory[this.navigLevel1].categoriesLevel2[this.navigLevel2].categoriesLevel3[this.navigLevel3].id;

                const ids = [id1, id2, id3];

                try {
                    await axios.post('/ProjectCategory/DeleteCategory', ids)
                } catch (error) {
                    console.error('Помилка під час виклику методу DeleteCategory:', error);
                }
                this.Init();
            }
        },

        toggleModal(type, indexLevel1, indexLevel2, indexLevel3) {

            this.modalType = type;

            this.navigLevel1 = indexLevel1;
            this.navigLevel2 = indexLevel2;
            this.navigLevel3 = indexLevel3;


            if (indexLevel1 == -1) {
                this.modalName = '';
             }

            if (indexLevel1 !== -1 && indexLevel2 == -1) {
                this.modalName = this.filteredCategory[indexLevel1].name;
                this.modalCategoryUsed = this.filteredCategory[indexLevel1].projects.length !== 0;
                this.projectsUsed = this.filteredCategory[indexLevel1].projects;
            }

            if (indexLevel1 !== -1 && indexLevel2 !== -1) {
                this.modalName = this.filteredCategory[indexLevel1].categoriesLevel2[indexLevel2].name;
                this.modalCategoryUsed = this.filteredCategory[indexLevel1].categoriesLevel2[indexLevel2].projects.length !== 0;
                this.projectsUsed = this.filteredCategory[indexLevel1].categoriesLevel2[indexLevel2].projects;
            }

            if (indexLevel1 !== -1 && indexLevel2 !== -1 && indexLevel3 !== -1) {
                this.modalName = this.filteredCategory[indexLevel1].categoriesLevel2[indexLevel2].categoriesLevel3[indexLevel3].name;
                this.modalCategoryUsed = this.filteredCategory[indexLevel1].categoriesLevel2[indexLevel2].categoriesLevel3[indexLevel3].projects.length !== 0;
                this.projectsUsed = this.filteredCategory[indexLevel1].categoriesLevel2[indexLevel2].categoriesLevel3[indexLevel3].projects;
            }

            if (type === 1) {
                this.modalCategoryUsed = false;
                this.modalOperation = 'Ви впевнені, що хочете додати категорію в структуру? ' + this.modalName;
                this.modalTitle = 'Додавання категорії';
            }
            if (type === 2) {
                this.modalCategoryUsed = false;
                this.modalOperation = 'Ви впевнені, що хочете редагувати категорію проекту? ' + this.modalName;
                this.modalTitle = 'Редагування категорії';
                this.editCategoryName = this.modalName;
            }
            if (type === 3) {
                if (!this.modalCategoryUsed) {
                    this.modalOperation = 'Ви впевнені, що хочете видалити категорію проекту? ' + this.modalName;
                } else {
                    this.modalOperation = 'Категорія з назвою ' + this.modalName + ' використовується в проектах:\n' + this.projectsUsed.join('\n') + '. Видалення не буде виконане';;
                }
                this.modalTitle = 'Видалення категорії';
            }

        },
        closeAllAccordions() {
            const accordionItems = document.querySelectorAll(".accordion-collapse");
            accordionItems.forEach(item => {
                item.classList.remove("show");
            });
        },
    },
});
