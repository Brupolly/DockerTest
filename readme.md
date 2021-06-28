# A small example project
This project exists to test possibility of connecting to a RabbitMQ container with the default credentials (`localhost@guest:guest`)

It reqiures a RabbitMQ container to work:
```docker
docker run --name rabbitmq --hostname rabbitmq -p 15672:15672 -p 5672:5672 rabbitmq:management
```

Alternatively, if you don't need the web UI for RabbitMQ:
```docker
docker run --name rabbitmq --hostname rabbitmq -p 5672:5672 rabbitmq```