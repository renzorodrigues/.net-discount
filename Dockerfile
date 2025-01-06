# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copiar apenas os arquivos do servidor
COPY ./DiscountGRPCServer ./DiscountGRPCServer

# Mudar para o diretório do servidor
WORKDIR /app/DiscountGRPCServer

# Publicar o projeto
RUN dotnet publish DiscountGRPCServer.csproj -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/DiscountGRPCServer/out .
ENTRYPOINT ["dotnet", "DiscountGRPCServer.dll"]


