using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestTask.DAL.Models;
using TestTask.DTO;
using TestTask.DTO.Exceptions;

namespace TestTask.DAL.Repositories
{
    public class ContractRepository : RepositoryBase<Contract>
    {
        public ContractRepository(TestTaskDbContext dbContext) : base(dbContext) { }

        public async Task<List<ContractDto>> GetAllAsync()
        {
            var contracts = await DbContext.Contracts.Include(c => c.Product).Include(c => c.ProductionRoom).ToListAsync();
            if (contracts.Any())
            {
                return contracts.Select(c => new ContractDto()
                {
                    Id = c.Id,
                    ProductQuantity = c.ProductQuantity,
                    Product = new ProductDto() 
                    {
                        Id = c.Product.Id,
                        Name = c.Product.Name,
                        Size = c.Product.Size
                    },
                    ProductionRoom = new ProductionRoomDto() 
                    {
                        Id = c.ProductionRoom.Id,
                        Name = c.ProductionRoom.Name,
                        Space = c.ProductionRoom.Space
                    }
                    
                }).ToList();
            }

            return new List<ContractDto>();
        }

        public async Task<string> AddOrUpdateAsync(ContractDto dto)
        {
            if (dto == null || dto.Product == null || dto.ProductionRoom == null)
            {
                throw new BadRequestException("Model must be specified");
            }

            Contract contract;
            if (dto.Id != Guid.Empty)
            {
                contract = await DbContext.Contracts.FirstOrDefaultAsync(c => c.Id == dto.Id);
                if (contract == null)
                {
                    throw new NotFoundException("Contract was not found");
                }
            }
            else
            {
                contract = new Contract
                {
                    ProductQuantity = dto.ProductQuantity
                };
            }

            if (dto.Product.Id != Guid.Empty)
            {
                contract.Product = await DbContext.Products.FirstOrDefaultAsync(p => p.Id == dto.Product.Id);
                if (contract.Product == null)
                {
                    throw new NotFoundException("Product was not found");
                }
            }
            else 
            {
                contract.Product = new Product
                {
                    Name = dto.Product.Name,
                    Size = dto.Product.Size
                };
            }

            if (dto.ProductionRoom.Id != Guid.Empty)
            {
                contract.ProductionRoom = await DbContext.ProductionRooms.FirstOrDefaultAsync(pr => pr.Id == dto.ProductionRoom.Id);
                if (contract.ProductionRoom == null)
                {
                    throw new NotFoundException("ProductionRoom was not found");
                }

                contract.ProductQuantity = dto.ProductQuantity;
            }
            else
            {
                contract.ProductionRoom = new ProductionRoom
                {
                    Name = dto.ProductionRoom.Name,
                    Space = dto.ProductionRoom.Space
                };
            }

            var v1 = contract.Product.Size * contract.ProductQuantity;
            var v2 = contract.ProductionRoom.Space;

            if (contract.Product.Size * contract.ProductQuantity > contract.ProductionRoom.Space)
            {
                throw new BadRequestException("ProductRoom space cannot accommodate this amount of Product");
            }

            if (contract.Id == Guid.Empty)
            {
                if (await base.CreateAsync(contract))
                {
                    return "Contract successfully added";
                }
            }
            else
            {
                if (await base.UpdateAsync(contract))
                {
                    return $"Contract [{contract.Id}] successfully updated";
                }
            }

            throw new SaveDataException("Contract not added due to internal service error");
        }
    }
}
