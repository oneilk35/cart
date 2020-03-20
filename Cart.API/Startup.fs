namespace Cart.API

open Microsoft.AspNetCore.Mvc.NewtonsoftJson
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open System.IO
open System.Reflection


type Startup private () =
    new (configuration: IConfiguration) as this =
        Startup() then
        this.Configuration <- configuration

    // This method gets called by the runtime. Use this method to add services to the container.
    member this.ConfigureServices(services: IServiceCollection) =
        // Add framework services.
        services
            .AddMvc(fun opt -> opt.EnableEndpointRouting <- false)
            .AddNewtonsoftJson(fun opt ->
                opt.SerializerSettings.Converters.Add(FsCodec.NewtonsoftJson.OptionConverter())
                opt.SerializerSettings.Converters.Add(FsCodec.NewtonsoftJson.UnionConverter())
                opt.SerializerSettings.Converters.Add(FsCodec.NewtonsoftJson.TypeSafeEnumConverter())
                opt.SerializerSettings.Converters.Add(FsCodec.NewtonsoftJson.VerbatimUtf8JsonConverter())
            )
            |> ignore
        services.AddControllers() |> ignore

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    member this.Configure(app: IApplicationBuilder, env: IWebHostEnvironment) =
        if (env.IsDevelopment()) then
            app.UseDeveloperExceptionPage() |> ignore

        app.UseHttpsRedirection() |> ignore
        app.UseRouting() |> ignore

        app.UseAuthorization() |> ignore

        app.UseEndpoints(fun endpoints ->
            endpoints.MapControllers() |> ignore
            ) |> ignore

    member val Configuration : IConfiguration = null with get, set
