start:
	docker-compose up -d

stop:
	docker-compose down

logs:
	docker-compose logs -f api


rebuild:
	docker-compose build api
	docker-compose up -d api

reset:
	docker-compose down -v && docker-compose up -d