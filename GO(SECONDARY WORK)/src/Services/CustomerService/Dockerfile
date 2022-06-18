FROM golang:latest
RUN mkdir /app
ENV GO_ENV dev
ADD . /app
WORKDIR /app
RUN go build -o main .
EXPOSE 5000
CMD ["/app/main"]
