using AutoMapper;

using LanguageExt;

using System.Collections.Generic;
using System.Linq;

using UserLoader.DbModel;
using UserLoader.DbModel.Models;

namespace UserLoader.Operations
{
    public class UserOperations : IUserOperations
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<UserEntity> _repository;
        private readonly IMapper _mapper;

        public UserOperations(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _repository = unitOfWork.GetRepository<UserEntity>();
            _mapper = mapper;
        }
        public Try<IEnumerable<UserModel>> GetAllUsers() => _repository.All.Map(entities => entities.Select(_mapper.Map<UserModel>));

        public Try<Unit> Insert(UserModel model) => _repository.Insert(_mapper.Map<UserEntity>(model));
    }
}
