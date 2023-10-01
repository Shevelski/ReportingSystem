
new Vue({
    el: '#Projects',
    data: {
        searchQueryProject: '',
        pageCount: 1,
        pageCur: 1,
        itemsPerPage: 10,
        modalType: 0,
        modalIndex: null,
        indexProject: 0,
        modalName: '',
        modalOperation: null,
        modalTitle: null,
        modalProjectActive: false,
        editProjectName: '',
    },
    mounted() {
        this.Init();
    },
    computed: {
        countFilteredProjects() {
            const nameFilter = this.searchQueryProject ? this.searchQueryProject.toLowerCase() : '';

            let filteredList = this.projects.filter((project) => {
                const nameMatches = !nameFilter || project.name.toLowerCase().includes(nameFilter);
                return nameMatches;
            });
            return filteredList.length;
        },
        filteredProjects() {
            const nameFilter = this.searchQueryProject ? this.searchQueryProject.toLowerCase() : '';

            let filteredList = this.projects.filter((project) => {
                const nameMatches = !nameFilter || project.name.toLowerCase().includes(nameFilter);
                return nameMatches;

            });

            if (this.pageCur === 1) {
                return filteredList.slice(0, this.pageCur * this.itemsPerPage);
            } else {
                return filteredList.slice(this.pageCur * this.itemsPerPage - this.itemsPerPage, this.pageCur * this.itemsPerPage);
            }

        },
    },
    methods: {
        async Init() {
            let response = await axios.get("/Projects/GetProjects");
            console.log(response.data);
            this.projects = response.data;
        },
        setItemsPerPage(count) {
            this.itemsPerPage = count;
        },
        nextBatch(){
            this.pageCount = Math.ceil(this.countFilteredProjects / this.itemsPerPage);

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
        confirmCreateProject() {
            const newProject =
            {
                name: this.editProjectName,
                description: '',
                head: '',
                startDate: new Date(),
                endDate: new Date(),
                projectCostsForCompany: 0,
                projectCostsForCustomer: 0,
                members: [],
                steps: [],
            }

            this.projects.push(newProject)
            this.editProjectName = '';

        },
        confirmEditProject() {
            console.log(this.indexProject);
            console.log(this.editProjectName);
            this.filteredProjects[this.indexProject].name = this.editProjectName;
        },
        confirmDeleteProject() {
            if (!this.modalProjectActive) {
                this.projects.splice(this.indexProject + this.pageCur * this.itemsPerPage - this.itemsPerPage, 1);
                this.indexProject = -1;
            }
        },

        toggleModal(type, index) {

            this.modalType = type;
            this.indexProject = index;

            if (type === 1) {
                this.modalProjectActive = false;
                this.modalOperation = 'Ви впевнені, що хочете створити проект? ' + this.modalName;
                this.modalTitle = 'Створення нового проекту';
                this.editProjectName = this.modalName;
            }

            if (type === 2) {
                this.modalName = this.filteredProjects[index].name;
                this.modalProjectActive = false;
                this.modalOperation = 'Ви впевнені, що хочете редагувати проект? ' + this.modalName;
                this.modalTitle = 'Редагування проекту';
                this.editProjectName = this.modalName;
            }
            if (type === 3) {
                this.modalName = this.filteredProjects[index].name;
                if (!this.modalProjectActive) {
                    this.modalOperation = 'Ви впевнені, що хочете видалити проект? ' + this.modalName;
                } else {
                    this.modalOperation = 'Проект з назвою ' + this.modalName + ' не має статусу - Для видалення -:\n' + this.projectsUsed.join('\n') + '. Видалення не буде виконане.';
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
