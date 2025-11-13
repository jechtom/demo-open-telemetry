FROM maven:3-eclipse-temurin-25-noble AS build

WORKDIR /app

COPY pom.xml .
COPY src ./src

RUN mvn clean package -DskipTests

FROM openjdk:25-rc

WORKDIR /app
COPY --from=build /app/target/catalog-service-*-SNAPSHOT.jar app.jar

EXPOSE 8080

ENTRYPOINT ["java", "-jar", "app.jar"]