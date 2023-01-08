using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.FileParser;
using FLChat.FileParser.Exceptions;
using FLChat.WebService.DataTypes;

namespace FLChat.WebService.Handlers.File
{
    public class SetPhoneFile : IObjectedHandlerStrategy<SetPhoneFileRequest, SetPhoneFileResponse>
    {
        private readonly IPhonesSaver _phonesSaver;
        private readonly IPhonesGetter _phonesGetter;
        private readonly IPhoneFileParser _parser;

        public bool IsReusable => true;

        public SetPhoneFile(IPhoneFileParser parser = null, IPhonesGetter phonesGetter = null, IPhonesSaver phonesSaver = null)
        {
            _parser = parser ?? new PhoneFileParser();
            _phonesSaver = phonesSaver ?? new PhonesSaver();
            _phonesGetter = phonesGetter ?? new PhonesGetter();
        }

        public SetPhoneFileResponse ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, SetPhoneFileRequest input)
        {
            VerifyInput(input);
            var result = new SetPhoneFileResponse();
            try
            {
                List<string> phones = _parser.Parse(input.FileData, input.FileName);
                List<Guid> guids = _phonesGetter.GetMatchedPhones(entities, currUserInfo.UserId, phones).ToList();
                _phonesSaver.Save(entities, currUserInfo.UserId, guids);

                result.PhonesCount = phones.Count;
                result.UsersCount = guids.Count;
            }
            catch (WrongFileTypeException e)
            {
                throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.input_data_error, $"Wrong file type ({e.FileName})");
            }

            return result;
        }

        private void VerifyInput(SetPhoneFileRequest input)
        {
            if (string.IsNullOrWhiteSpace(input.FileData))
                throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.input_data_error, $"Field [data] can't be empty");
            if (string.IsNullOrWhiteSpace(input.FileName))
                throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.input_data_error, $"Field [file_name] can't be empty");
        }
    }
}
