using AutoMapper;

using LanguageExt;

using System.Collections.Generic;
using System.Linq;

using UserLoader.DbModel;
using UserLoader.DbModel.Models;

namespace UserLoader.Operations
{
    public class UserOperations : IUserReader, IUserWriter
    {
        private readonly IRepository<UserEntity> _repository;
        private readonly IMapper _mapper;

        public UserOperations(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = unitOfWork.GetRepository<UserEntity>();
            _mapper = mapper;
        }
        public Try<IEnumerable<UserModel>> GetAllUsers() => _repository.All.Map(entities => entities.Select(_mapper.Map<UserModel>));

        public Try<UserModel> Insert(UserModel model)
        {
            var mapped = _mapper.Map<UserEntity>(model);
            mapped.CreatedDate = System.DateTime.UtcNow.Date;
            model.CreatedDate = new System.DateTimeOffset(mapped.CreatedDate, System.TimeSpan.Zero);

            return _repository.Insert(mapped).Map(_ => model);
        }
    }
}
