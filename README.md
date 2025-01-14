# dormy
Migrate db:
1. docker-compose down -v
2. docker-compose up --build
3. remove-migration
4. add-migration InitDb
5. update-database