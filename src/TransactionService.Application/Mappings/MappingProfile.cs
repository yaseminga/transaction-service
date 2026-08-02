using AutoMapper;
using TransactionService.Application.DTOs.Transactions;
using TransactionService.Application.DTOs.Users;
using TransactionService.Domain.Entities;
using TransactionService.Domain.QueryModels;

namespace TransactionService.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User
            CreateMap<CreateUserRequest, User>();
            CreateMap<UpdateUserRequest, User>();
            CreateMap<User, UserResponse>();

            // Transaction
            CreateMap<CreateTransactionRequest, Transaction>();
            CreateMap<Transaction, TransactionResponse>()
                .ForMember(
                    dest => dest.UserName,
                    opt => opt.MapFrom(src => src.User.Name));

            // Summary
            CreateMap<UserTransactionSummary, UserTransactionSummaryResponse>();
            CreateMap<TransactionTypeSummary, TransactionTypeSummaryResponse>();
        }
    }
}
