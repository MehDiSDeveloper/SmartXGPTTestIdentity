using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using OpenAI.Assistants;
using OpenAI;
using OpenAI_API;
using OpenAI_API.Chat;
using SmartXGPT.Service.Interfaces;
using SmartXGPT.Service.Models.DTOs;
using SmartXGPT.Service.Models.Entities;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using OpenAI.Threads;
using OpenAI_API.Completions;
using OpenAI.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Newtonsoft.Json;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;
using SmartXGPT.Infrastructure.Contexts.SmartXDbContext;
using AutoMapper;
using OpenAI.Chat;


namespace SmartXGPT.Infrastructure.APIServices
{
    public class GPTAPIService : IGPTAPIService
    {
        private readonly IConfiguration _configuration;
        private readonly OpenAIClient _apiClient;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        private readonly string _assistantId = "asst_7S0xE1LvRSrnhwz30gr7MiSm";


        public GPTAPIService(IConfiguration configuration, ApplicationDbContext applicationDbContext,IMapper mapper)
        {
            _configuration = configuration;
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
            _apiClient = new OpenAIClient(_configuration.GetSection("Appsettings:GChatClubAPIKEY").Value);
        }


        public async Task<string> SetEvent(EventDto dto)
        {
            Service.Models.Entities.Event ev = _mapper.Map<Service.Models.Entities.Event>(dto);
            
        
            await _applicationDbContext.Events.AddAsync(ev);
            await _applicationDbContext.SaveChangesAsync();

            string res = $"آقای {dto.creatorLastName}ایونت رستوران {dto.RestaurantName} در تاریخ {dto.MessagesSendDate} برای مشتریان گروه {dto.customerType} در ساعت {dto.MessagesSendHour} ثبت شد. \n" +
                $"متن پیام ارسالی : \" {dto.MessagePassage}\" \n";
            if (dto.SendExpireNotification)
                res += $"\n همچنین در زمان انقضا پیام یادآوری ارسال خواهد شد";
            else
                res += $"همچنین در زمان انقضا پیام یادآوری ارسال نخواهد شد";
            return res;
        }




        public string GetWeather(WeatherDto dto)
        {
            if (dto.Location.ToLower() == "kuala lumpur")
                return "weather in Kuala-Lumpur is 23 degree";
            else return "i dont have the weather information";

        }




        public string GetNumber(CellNumberDto dto)
        {
            if (dto.Name.ToLower() == "mehdi" || dto.Name == "مهدی")
                return "mehdi phone number is 09125850371";
            else return "i dont have his number";
        }


        //public string InvokeMethod(string methodName, weatherDto dto)
        //{
        //    // Get the method info using reflection
        //    var methodInfo = typeof(IGPTAPIService).GetMethod(methodName);

        //    if (methodInfo == null)
        //    {
        //        return "Invalid method name";
        //    }

        //    // Invoke the method with the provided DTO
        //    try
        //    {
        //        methodInfo.Invoke(this, new object[] { dto });
        //        return "Method invoked successfully";
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle any exceptions that may occur during method invocation
        //        return $"Method invocation failed: {ex.Message}";
        //    }
        //}



        public async Task<AssistantReqDto> AskGPTAssistantAsync(AssistantReqDto assistantReqDto)
        {
            try
            {

                //var getTheNumber = new OpenAI.Function(
                //    "getNumber",
                //    "get the phone number of a person using his name",
                //    new JsonObject
                //    {
                //        ["type"] = "object",
                //        ["properties"] = new JsonObject
                //        {
                //            ["name"] = new JsonObject
                //            {
                //                ["type"] = "string",
                //                ["description"] = "the name of the person to retrive his number"
                //            },
                //            ["userId"] = new JsonObject
                //            {
                //                ["type"] = "string",
                //                ["description"] = "the Id of asking user that assistant must fill this from provided id by system and dont ask this from user"
                //            }
                //        },
                //        ["required"] = new JsonArray { "name" , "userId"}
                //    });



                //var getTheWeather = new OpenAI.Function(
                //    "getWeather",
                //    "Get the current weather in a given location",
                //    new JsonObject
                //    {
                //        ["type"] = "object",
                //        ["properties"] = new JsonObject
                //        {
                //            ["location"] = new JsonObject
                //            {
                //                ["type"] = "string",
                //                ["description"] = "The city and state, e.g. San Francisco, CA"
                //            },
                //            ["unit"] = new JsonObject
                //            {
                //                ["type"] = "string",
                //                ["enum"] = new JsonArray { "celsius", "fahrenheit" }
                //            },
                //            ["userId"] = new JsonObject
                //            {
                //                ["type"] = "string",
                //                ["description"] = "the Id of asking user that assistant must fill this from provided id by system and dont ask this from user"
                //            }

                //        },
                //        ["required"] = new JsonArray { "location", "unit","userId" }
                //    });



                //var assistant = await api.AssistantsEndpoint.CreateAssistantAsync(

                //    new CreateAssistantRequest(
                //        name: "Math Tutor",
                //        instructions: "You are a personal math tutor. Answer questions briefly, in a sentence or less.",
                //        model: "gpt-4-1106-preview",
                //        tools: new Tool[] { func })
                //        ));





                // var testAssistant = await _apiClient.AssistantsEndpoint.CreateAssistantAsync(new CreateAssistantRequest(tools: new Tool[] { getTheWeather, getTheNumber}));
                //_apiClient.AssistantsEndpoint.EnableDebug = true;
                ThreadResponse thread;
                var testAssistant = await _apiClient.AssistantsEndpoint.RetrieveAssistantAsync("asst_qNazakCOoN1AKPyUs3fFZwPJ");
                if (string.IsNullOrEmpty(assistantReqDto.ThreadId))
                {

                    thread = await _apiClient.ThreadsEndpoint.CreateThreadAsync();
                    assistantReqDto.ThreadId = thread.Id;
                    var sysMessage = await thread.CreateMessageAsync("system:userId=uSeR1234");

                }
                else
                {
                    thread = await _apiClient.ThreadsEndpoint.RetrieveThreadAsync(assistantReqDto.ThreadId);

                    //thread = await thread.UpdateAsync();
                }

                //var api = new OpenAIClient(_configuration.GetSection("Appsettings:GChatClubAPIKEY").Value);
                //var assistant = await _apiClient.AssistantsEndpoint.RetrieveAssistantAsync("asst_qNazakCOoN1AKPyUs3fFZwPJ");
                //var filePath = "files/assistant_test_2.txt.txt";
                //await File.WriteAllTextAsync(filePath, "answers for questions about smartx ");
                //var assistantFile = await assistant.UploadFileAsync(filePath);
                var userMessage = await thread.CreateMessageAsync(assistantReqDto.Question);
                RunResponse run ;
                //run = await thread.CreateRunAsync(testAssistant);
                //assistantReqDto.RunId = run.Id;
                if (string.IsNullOrEmpty(assistantReqDto.RunId))
                {
                    run = await thread.CreateRunAsync(testAssistant);
                    assistantReqDto.RunId = run.Id;
                }
                else
                { run = await _apiClient.ThreadsEndpoint.RetrieveRunAsync(thread.Id, assistantReqDto.RunId); }


                //var rusns = thread.ListRunsAsync();
                run = await run.WaitForStatusChangeAsync();
                //var x = run.RemainingRequests;
                //var y=run.ListRunStepsAsync;


                //run.Tools.Append(getTheWeather);
                // if action is required
                if (run.RequiredAction != null)
                {
                    var type = run.RequiredAction.SubmitToolOutputs.ToolCalls[0].Type;

                    var toolCall = run.RequiredAction.SubmitToolOutputs.ToolCalls[0];
                    var funcName = toolCall.FunctionCall.Name;
                    //var methodInfo = typeof(IGPTAPIService).GetMethod(funcName);
                    string result = "";
                    //dynamic dynamicDto = JsonConvert.DeserializeObject<dynamic>(toolCall.FunctionCall.Arguments);
                    switch (funcName)
                    {
                        case "GetWeather":

                            // call the method with the provided DTO
                            try
                            {

                                result = GetWeather(JsonConvert.DeserializeObject<WeatherDto>(toolCall.FunctionCall.Arguments));
                                break;
                            }
                            catch (Exception ex)
                            {
                                // Handle any exceptions that may occur during method invocation
                                result = $"Method invocation failed: {ex.Message}";
                            }
                            break;


                        case "GetNumber":

                            // call the method with the provided DTO
                            try
                            {
                                result = GetNumber(JsonConvert.DeserializeObject<CellNumberDto>(toolCall.FunctionCall.Arguments));
                                break;
                            }
                            catch (Exception ex)
                            {
                                // Handle any exceptions that may occur during method invocation
                                result = $"Method invocation failed: {ex.Message}";
                            }
                            break;


                        case "SetEvent":

                            // call the method with the provided DTO
                            try
                            {
                                result = SetEvent(JsonConvert.DeserializeObject<EventDto>(toolCall.FunctionCall.Arguments)).Result;
                                break;
                            }
                            catch (Exception ex)
                            {
                                // Handle any exceptions that may occur during method invocation
                                result = $"Method invocation failed: {ex.Message}";
                            }
                            break;
                    }

                    //implimentation of method callerf

                    var toolOutput = new ToolOutput(toolCall.Id, result);
                    run = await run.SubmitToolOutputsAsync(toolOutput);
                    // waiting while run in Queued and InProgress
                    run = await run.WaitForStatusChangeAsync();
                }

                var messages = await run.ListMessagesAsync();
                var lastMessage = messages.Items[0];
                if (lastMessage.Role == Role.Assistant)
                {
                    assistantReqDto.Answer = messages.Items[0].Content[0].ToString();
                }
                else
                {
                    assistantReqDto.Answer = "NO ANSWER";
                }

                if (messages.Items.Count > 20)
                {
                    var message = new CreateMessageRequest("system: userId=user1234", null, null);
                    var sysMessage = await thread.CreateMessageAsync(message);
                }
                return assistantReqDto;

            }
            catch (Exception ex)
            {
                assistantReqDto.Answer = ex.Message;
                return assistantReqDto;
            }
        }



        async Task<MesssageGPTDto> IGPTAPIService.AskGPTAsync(MesssageGPTDto messageDto)
        {
            try
            {
                string prompt = messageDto.AskThis;

                MesssageGPTDto response = messageDto;

                var apiKey = _configuration.GetSection("Appsettings:GChatClubAPIKEY").Value;
                var apiModel = _configuration.GetSection("Appsettings:ClubModel").Value;
                string rs = string.Empty;
                OpenAIAPI api = new(new APIAuthentication(apiKey));
                var openAi = new OpenAIAPI(apiKey);



                var chatRequest = new OpenAI_API.Chat.ChatRequest()
                {

                    Messages = new ChatMessage[] {
                        new ChatMessage(ChatMessageRole.User, prompt)
                    },
                    Model = "gpt-4-1106-preview",
                    Temperature = messageDto.Temperature,
                    MaxTokens = 1000,
                    TopP = 1.0,
                    FrequencyPenalty = 0.0,
                    PresencePenalty = 0.0,


                };
                var APIresult = await api.Chat.CreateChatCompletionAsync(chatRequest);

                rs = APIresult.ToString();
                if (string.IsNullOrEmpty(rs))
                {
                    response.IsSuccess = false;
                    response.ErrorMessage = "پاسخی از جی پی تی دریافت نشد";
                    return response;
                }
                response.RawAnswer = rs;
                response.IsSuccess = true;
                response.IsAsked = true;
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
