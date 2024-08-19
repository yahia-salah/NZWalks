using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using NZWalks.API.Application.Services;
using NZWalks.API.Data;
using NZWalks.API.Models.Domains;
using NZWalks.API.Models.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Shouldly;

namespace NZWalks.Tests
{
	public class HoldDateAppServiceTest
	{
		private readonly HoldDateAppService _sut;
		private readonly Mock<IptsdbContext> _dbContext = new Mock<IptsdbContext>();
		private readonly Mock<IMapper> _mapper = new Mock<IMapper>();

		public HoldDateAppServiceTest()
		{
			_sut = new HoldDateAppService(_dbContext.Object, _mapper.Object);
		}

		[Fact]
		public async Task GetAllHoldDates_ShouldReturnThreeHoldDates()
		{
			// Arrange
			var holdDates = new List<HoldDate>
			{
				new HoldDate { Id = 1, Name = "May - June",Description="All new escape enrollment", BeginHold = new DateTime(2024, 5, 1), Release = new DateTime(2024, 7, 1) },
				new HoldDate { Id = 2, Name = "June - July",Description="All existing annual / escape downward corrections", BeginHold = new DateTime(2024, 6, 1), Release = new DateTime(2024, 7, 1) },
				new HoldDate { Id = 3, Name = "July - Sep",Description="Current year escape / annual corrections", BeginHold = new DateTime(2024, 6, 1), Release = new DateTime(2024, 9, 1) }
			}.AsQueryable();

			var mockSet = new Mock<DbSet<HoldDate>>();
			mockSet.As<IQueryable<HoldDate>>().Setup(m => m.Provider).Returns(holdDates.Provider);
			mockSet.As<IQueryable<HoldDate>>().Setup(m => m.Expression).Returns(holdDates.Expression);
			mockSet.As<IQueryable<HoldDate>>().Setup(m => m.ElementType).Returns(holdDates.ElementType);
			mockSet.As<IQueryable<HoldDate>>().Setup(m => m.GetEnumerator()).Returns(holdDates.GetEnumerator());

			_dbContext.Setup(x => x.Set<HoldDate>()).Returns(mockSet.Object);

			_mapper.Setup(x => x.Map<HoldDateDto>(It.IsAny<HoldDate>())).Returns((HoldDate h) => new HoldDateDto { Id = h.Id, Name = h.Name, Description = h.Description, BeginHold = h.BeginHold, Release = h.Release });

			// Act
			var result = await _sut.GetAllAsync();

			// Assert
			result.Count().ShouldBe(3);
			result.ElementAt(0).Name.ShouldBe("May - June");
			result.ElementAt(0).Id.ShouldBe(1);
			result.ElementAt(0).Description.ShouldBe("All new escape enrollment");
			result.ElementAt(0).BeginHold.ShouldBe(new DateTime(2024, 5, 1));
			result.ElementAt(0).Release.ShouldBe(new DateTime(2024, 7, 1));
			result.ElementAt(1).Name.ShouldBe("June - July");
			result.ElementAt(1).Id.ShouldBe(2);
			result.ElementAt(1).Description.ShouldBe("All existing annual / escape downward corrections");
			result.ElementAt(1).BeginHold.ShouldBe(new DateTime(2024, 6, 1));
			result.ElementAt(1).Release.ShouldBe(new DateTime(2024, 7, 1));
			result.ElementAt(2).Name.ShouldBe("July - Sep");
			result.ElementAt(2).Id.ShouldBe(3);
			result.ElementAt(2).Description.ShouldBe("Current year escape / annual corrections");
			result.ElementAt(2).BeginHold.ShouldBe(new DateTime(2024, 6, 1));
			result.ElementAt(2).Release.ShouldBe(new DateTime(2024, 9, 1));
		}

		[Fact]
		public async Task UpdateAllAsync_ShouldUpdateRecordsCorrectly()
		{
			// Arrange
			var holdDatesUpdateDto = new HoldDatesUpdateDto
			{
				Updates = new List<HoldDateUpdateDto>
				{
					new HoldDateUpdateDto { Name = "May - June", BeginHold = new DateTime(2024, 5, 15), Release = new DateTime(2024, 7, 15) },
					new HoldDateUpdateDto { Name = "June - July", BeginHold = new DateTime(2024, 6, 15), Release = new DateTime(2024, 7, 15) },
					new HoldDateUpdateDto { Name = "July - Sep", BeginHold = new DateTime(2024, 6, 15), Release = new DateTime(2024, 9, 15) }
				}
			};

			var existingRecords = new List<HoldDate>
			{
				new HoldDate { Id = 1, Name = "May - June", BeginHold = new DateTime(2024, 5, 1), Release = new DateTime(2024, 7, 1) },
				new HoldDate { Id = 2, Name = "June - July", BeginHold = new DateTime(2024, 6, 1), Release = new DateTime(2024, 7, 1) },
				new HoldDate { Id = 3, Name = "July - Sep", BeginHold = new DateTime(2024, 6, 1), Release = new DateTime(2024, 9, 1) }
			};

			var mockSet = new Mock<DbSet<HoldDate>>();
			mockSet.As<IQueryable<HoldDate>>().Setup(m => m.Provider).Returns(existingRecords.AsQueryable().Provider);
			mockSet.As<IQueryable<HoldDate>>().Setup(m => m.Expression).Returns(existingRecords.AsQueryable().Expression);
			mockSet.As<IQueryable<HoldDate>>().Setup(m => m.ElementType).Returns(existingRecords.AsQueryable().ElementType);
			mockSet.As<IQueryable<HoldDate>>().Setup(m => m.GetEnumerator()).Returns(existingRecords.AsQueryable().GetEnumerator());
			mockSet.As<IAsyncEnumerable<HoldDate>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>())).Returns(new TestAsyncEnumerator<HoldDate>(existingRecords.GetEnumerator()));

			_dbContext.Setup(x => x.Set<HoldDate>()).Returns(mockSet.Object);

			_mapper.Setup(x => x.Map<List<HoldDateDto>>(It.IsAny<List<HoldDate>>()))
				.Returns((List<HoldDate> source) => source.Select(h => new HoldDateDto
				{
					Id = h.Id,
					Name = h.Name,
					BeginHold = h.BeginHold,
					Release = h.Release,
					Description = h.Description
				}).ToList());

			// Act
			var result = await _sut.UpdateAllAsync(holdDatesUpdateDto);

			// Assert
			result.Count.ShouldBe(3);
			result.First(r => r.Name == "May - June").BeginHold.ShouldBe(new DateTime(2024, 5, 15));
			result.First(r => r.Name == "May - June").Release.ShouldBe(new DateTime(2024, 7, 15));
			result.First(r => r.Name == "June - July").BeginHold.ShouldBe(new DateTime(2024, 6, 15));
			result.First(r => r.Name == "June - July").Release.ShouldBe(new DateTime(2024, 7, 15));
			result.First(r => r.Name == "July - Sep").BeginHold.ShouldBe(new DateTime(2024, 6, 15));
			result.First(r => r.Name == "July - Sep").Release.ShouldBe(new DateTime(2024, 9, 15));

			_dbContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
		}

		[Fact]
		public async Task UpdateAllAsync_ShouldSkipInvalidNameAndUpdateOnlyMatchedRecords()
		{
			// Arrange
			var holdDatesUpdateDto = new HoldDatesUpdateDto
			{
				Updates = new List<HoldDateUpdateDto>
				{
					new HoldDateUpdateDto { Name = "xyz", BeginHold = new DateTime(2024, 5, 15), Release = new DateTime(2024, 7, 15) },
					new HoldDateUpdateDto { Name = "June - July", BeginHold = new DateTime(2024, 6, 15), Release = new DateTime(2024, 7, 15) },
					new HoldDateUpdateDto { Name = "July - Sep", BeginHold = new DateTime(2024, 6, 15), Release = new DateTime(2024, 9, 15) }
				}
			};

			var existingRecords = new List<HoldDate>
			{
				new HoldDate { Id = 1, Name = "May - June", BeginHold = new DateTime(2024, 5, 1), Release = new DateTime(2024, 7, 1) },
				new HoldDate { Id = 2, Name = "June - July", BeginHold = new DateTime(2024, 6, 1), Release = new DateTime(2024, 7, 1) },
				new HoldDate { Id = 3, Name = "July - Sep", BeginHold = new DateTime(2024, 6, 1), Release = new DateTime(2024, 9, 1) }
			};

			var mockSet = new Mock<DbSet<HoldDate>>();
			mockSet.As<IQueryable<HoldDate>>().Setup(m => m.Provider).Returns(existingRecords.AsQueryable().Provider);
			mockSet.As<IQueryable<HoldDate>>().Setup(m => m.Expression).Returns(existingRecords.AsQueryable().Expression);
			mockSet.As<IQueryable<HoldDate>>().Setup(m => m.ElementType).Returns(existingRecords.AsQueryable().ElementType);
			mockSet.As<IQueryable<HoldDate>>().Setup(m => m.GetEnumerator()).Returns(existingRecords.AsQueryable().GetEnumerator());
			mockSet.As<IAsyncEnumerable<HoldDate>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>())).Returns(new TestAsyncEnumerator<HoldDate>(existingRecords.GetEnumerator()));

			_dbContext.Setup(x => x.Set<HoldDate>()).Returns(mockSet.Object);

			_mapper.Setup(x => x.Map<List<HoldDateDto>>(It.IsAny<List<HoldDate>>()))
				.Returns((List<HoldDate> source) => source.Select(h => new HoldDateDto
				{
					Id = h.Id,
					Name = h.Name,
					BeginHold = h.BeginHold,
					Release = h.Release,
					Description = h.Description
				}).ToList());

			// Act
			var result = await _sut.UpdateAllAsync(holdDatesUpdateDto);

			// Assert
			result.Count.ShouldBe(3);
			result.FirstOrDefault(r => r.Name == "xyz").ShouldBeNull();
			result.First(r => r.Name == "May - June").BeginHold.ShouldBe(new DateTime(2024, 5, 1));
			result.First(r => r.Name == "May - June").Release.ShouldBe(new DateTime(2024, 7, 1));
			result.First(r => r.Name == "June - July").BeginHold.ShouldBe(new DateTime(2024, 6, 15));
			result.First(r => r.Name == "June - July").Release.ShouldBe(new DateTime(2024, 7, 15));
			result.First(r => r.Name == "July - Sep").BeginHold.ShouldBe(new DateTime(2024, 6, 15));
			result.First(r => r.Name == "July - Sep").Release.ShouldBe(new DateTime(2024, 9, 15));

			_dbContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
		}

		[Fact]
		public async Task ValidateBeforeUpdateAsync_ShouldReturnValid_WhenUpdatesAreValid()
		{
			// Arrange
			var holdDatesUpdateDto = new HoldDatesUpdateDto
			{
				Updates = new List<HoldDateUpdateDto>
				{
					new HoldDateUpdateDto { Name = "May - June", BeginHold = new DateTime(2024, 5, 15), Release = new DateTime(2024, 7, 15) },
					new HoldDateUpdateDto { Name = "June - July", BeginHold = new DateTime(2024, 6, 15), Release = new DateTime(2024, 7, 15) },
					new HoldDateUpdateDto { Name = "July - Sep", BeginHold = new DateTime(2024, 6, 15), Release = new DateTime(2024, 9, 15) }
				}
			};

			var existingRecords = new List<HoldDate>
			{
				new HoldDate { Id = 1, Name = "May - June", BeginHold = new DateTime(2024, 5, 1), Release = new DateTime(2024, 7, 1) },
				new HoldDate { Id = 2, Name = "June - July", BeginHold = new DateTime(2024, 6, 1), Release = new DateTime(2024, 7, 1) },
				new HoldDate { Id = 3, Name = "July - Sep", BeginHold = new DateTime(2024, 6, 1), Release = new DateTime(2024, 9, 1) }
			};

			var mockSet = new Mock<DbSet<HoldDate>>();
			mockSet.As<IQueryable<HoldDate>>().Setup(m => m.Provider).Returns(existingRecords.AsQueryable().Provider);
			mockSet.As<IQueryable<HoldDate>>().Setup(m => m.Expression).Returns(existingRecords.AsQueryable().Expression);
			mockSet.As<IQueryable<HoldDate>>().Setup(m => m.ElementType).Returns(existingRecords.AsQueryable().ElementType);
			mockSet.As<IQueryable<HoldDate>>().Setup(m => m.GetEnumerator()).Returns(existingRecords.AsQueryable().GetEnumerator());
			mockSet.As<IAsyncEnumerable<HoldDate>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>())).Returns(new TestAsyncEnumerator<HoldDate>(existingRecords.GetEnumerator()));

			_dbContext.Setup(x => x.Set<HoldDate>()).Returns(mockSet.Object);

			// Act
			var result = await _sut.ValidateBeforeUpdateAsync(holdDatesUpdateDto);

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeEmpty();
		}

		[Fact]
		public async Task ValidateBeforeUpdateAsync_ShouldReturnInvalid_WhenUpdateHasInvalidName()
		{
			// Arrange
			var holdDatesUpdateDto = new HoldDatesUpdateDto
			{
				Updates = new List<HoldDateUpdateDto>
				{
					new HoldDateUpdateDto { Name = "xyz", BeginHold = new DateTime(2024, 5, 15), Release = new DateTime(2024, 7, 15) },
					new HoldDateUpdateDto { Name="May - June", BeginHold = new DateTime(2024, 5, 15), Release = new DateTime(2024, 7, 15)},
					new HoldDateUpdateDto { Name = "June - July", BeginHold = new DateTime(2024, 6, 15), Release = new DateTime(2024, 7, 15) },
					new HoldDateUpdateDto { Name = "July - Sep", BeginHold = new DateTime(2024, 6, 15), Release = new DateTime(2024, 9, 15) }
				}
			};

			var existingRecords = new List<HoldDate>
			{
				new HoldDate { Id = 1, Name = "May - June", BeginHold = new DateTime(2024, 5, 1), Release = new DateTime(2024, 7, 1) },
				new HoldDate { Id = 2, Name = "June - July", BeginHold = new DateTime(2024, 6, 1), Release = new DateTime(2024, 7, 1) },
				new HoldDate { Id = 3, Name = "July - Sep", BeginHold = new DateTime(2024, 6, 1), Release = new DateTime(2024, 9, 1) }
			};

			var mockSet = new Mock<DbSet<HoldDate>>();
			mockSet.As<IQueryable<HoldDate>>().Setup(m => m.Provider).Returns(existingRecords.AsQueryable().Provider);
			mockSet.As<IQueryable<HoldDate>>().Setup(m => m.Expression).Returns(existingRecords.AsQueryable().Expression);
			mockSet.As<IQueryable<HoldDate>>().Setup(m => m.ElementType).Returns(existingRecords.AsQueryable().ElementType);
			mockSet.As<IQueryable<HoldDate>>().Setup(m => m.GetEnumerator()).Returns(existingRecords.AsQueryable().GetEnumerator());
			mockSet.As<IAsyncEnumerable<HoldDate>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>())).Returns(new TestAsyncEnumerator<HoldDate>(existingRecords.GetEnumerator()));

			_dbContext.Setup(x => x.Set<HoldDate>()).Returns(mockSet.Object);

			// Act
			var result = await _sut.ValidateBeforeUpdateAsync(holdDatesUpdateDto);

			// Assert
			result.ShouldNotBeNull();
			result.Count.ShouldBe(1);
			result.First().IsValid.ShouldBeFalse();
			result.First().Message.ShouldBe("'xyz' is invalid Name");
		}

		[Fact]
		public async Task ValidateBeforeUpdateAsync_ShouldReturnInvalid_WhenExistingRecordIsMissingFromUpdate()
		{
			// Arrange
			var holdDatesUpdateDto = new HoldDatesUpdateDto
			{
				Updates = new List<HoldDateUpdateDto>
				{
					new HoldDateUpdateDto { Name = "June - July", BeginHold = new DateTime(2024, 6, 15), Release = new DateTime(2024, 7, 15) },
					new HoldDateUpdateDto { Name = "July - Sep", BeginHold = new DateTime(2024, 6, 15), Release = new DateTime(2024, 9, 15) }
				}
			};

			var existingRecords = new List<HoldDate>
			{
				new HoldDate { Id = 1, Name = "May - June", BeginHold = new DateTime(2024, 5, 1), Release = new DateTime(2024, 7, 1) },
				new HoldDate { Id = 2, Name = "June - July", BeginHold = new DateTime(2024, 6, 1), Release = new DateTime(2024, 7, 1) },
				new HoldDate { Id = 3, Name = "July - Sep", BeginHold = new DateTime(2024, 6, 1), Release = new DateTime(2024, 9, 1) }
			};

			var mockSet = new Mock<DbSet<HoldDate>>();
			mockSet.As<IQueryable<HoldDate>>().Setup(m => m.Provider).Returns(existingRecords.AsQueryable().Provider);
			mockSet.As<IQueryable<HoldDate>>().Setup(m => m.Expression).Returns(existingRecords.AsQueryable().Expression);
			mockSet.As<IQueryable<HoldDate>>().Setup(m => m.ElementType).Returns(existingRecords.AsQueryable().ElementType);
			mockSet.As<IQueryable<HoldDate>>().Setup(m => m.GetEnumerator()).Returns(existingRecords.AsQueryable().GetEnumerator());
			mockSet.As<IAsyncEnumerable<HoldDate>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>())).Returns(new TestAsyncEnumerator<HoldDate>(existingRecords.GetEnumerator()));

			_dbContext.Setup(x => x.Set<HoldDate>()).Returns(mockSet.Object);

			// Act
			var result = await _sut.ValidateBeforeUpdateAsync(holdDatesUpdateDto);

			// Assert
			result.ShouldNotBeNull();
			result.Count.ShouldBe(1);
			result.First().IsValid.ShouldBeFalse();
			result.First().Message.ShouldBe("'May - June' is not found in the update list");
		}

		[Fact]
		public async Task ValidateBeforeUpdateAsync_ShouldReturnInvalid_WhenUpdateBothBeginHoldAndReleaseToNull()
		{
			// Arrange
			var holdDatesUpdateDto = new HoldDatesUpdateDto
			{
				Updates = new List<HoldDateUpdateDto>
				{
					new HoldDateUpdateDto { Name = "May - June", BeginHold = null, Release = null },
					new HoldDateUpdateDto { Name = "June - July", BeginHold = new DateTime(2024, 6, 15), Release = new DateTime(2024, 7, 15) },
					new HoldDateUpdateDto { Name = "July - Sep", BeginHold = new DateTime(2024, 6, 15), Release = new DateTime(2024, 9, 15) }
				}
			};

			var existingRecords = new List<HoldDate>
			{
				new HoldDate { Id = 1, Name = "May - June", BeginHold = new DateTime(2024, 5, 1), Release = new DateTime(2024, 7, 1) },
				new HoldDate { Id = 2, Name = "June - July", BeginHold = new DateTime(2024, 6, 1), Release = new DateTime(2024, 7, 1) },
				new HoldDate { Id = 3, Name = "July - Sep", BeginHold = new DateTime(2024, 6, 1), Release = new DateTime(2024, 9, 1) }
			};

			var mockSet = new Mock<DbSet<HoldDate>>();
			mockSet.As<IQueryable<HoldDate>>().Setup(m => m.Provider).Returns(existingRecords.AsQueryable().Provider);
			mockSet.As<IQueryable<HoldDate>>().Setup(m => m.Expression).Returns(existingRecords.AsQueryable().Expression);
			mockSet.As<IQueryable<HoldDate>>().Setup(m => m.ElementType).Returns(existingRecords.AsQueryable().ElementType);
			mockSet.As<IQueryable<HoldDate>>().Setup(m => m.GetEnumerator()).Returns(existingRecords.AsQueryable().GetEnumerator());
			mockSet.As<IAsyncEnumerable<HoldDate>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>())).Returns(new TestAsyncEnumerator<HoldDate>(existingRecords.GetEnumerator()));

			_dbContext.Setup(x => x.Set<HoldDate>()).Returns(mockSet.Object);

			// Act
			var result = await _sut.ValidateBeforeUpdateAsync(holdDatesUpdateDto);

			// Assert
			result.ShouldNotBeNull();
			result.Count.ShouldBe(1);
			result.First().IsValid.ShouldBeFalse();
			result.First().Message.ShouldBe("Begin hold and Release cannot be empty for 'May - June'");
		}

		[Fact]
		public async Task ValidateBeforeUpdateAsync_ShouldReturnInvalid_WhenUpdateBeginHoldDateIsOutsideTheMonthFromRules()
		{
			// Arrange
			var holdDatesUpdateDto = new HoldDatesUpdateDto
			{
				Updates = new List<HoldDateUpdateDto>
				{
					new HoldDateUpdateDto { Name = "May - June", BeginHold = new DateTime(2024, 6, 15), Release = new DateTime(2024, 7, 15) },
					new HoldDateUpdateDto { Name = "June - July", BeginHold = new DateTime(2024, 5, 15), Release = new DateTime(2024, 7, 15) },
					new HoldDateUpdateDto { Name = "July - Sep", BeginHold = new DateTime(2024, 5, 15), Release = new DateTime(2024, 9, 15) }
				}
			};

			var existingRecords = new List<HoldDate>
			{
				new HoldDate { Id = 1, Name = "May - June", BeginHold = new DateTime(2024, 5, 1), Release = new DateTime(2024, 7, 1) },
				new HoldDate { Id = 2, Name = "June - July", BeginHold = new DateTime(2024, 6, 1), Release = new DateTime(2024, 7, 1) },
				new HoldDate { Id = 3, Name = "July - Sep", BeginHold = new DateTime(2024, 6, 1), Release = new DateTime(2024, 9, 1) }
			};

			var mockSet = new Mock<DbSet<HoldDate>>();
			mockSet.As<IQueryable<HoldDate>>().Setup(m => m.Provider).Returns(existingRecords.AsQueryable().Provider);
			mockSet.As<IQueryable<HoldDate>>().Setup(m => m.Expression).Returns(existingRecords.AsQueryable().Expression);
			mockSet.As<IQueryable<HoldDate>>().Setup(m => m.ElementType).Returns(existingRecords.AsQueryable().ElementType);
			mockSet.As<IQueryable<HoldDate>>().Setup(m => m.GetEnumerator()).Returns(existingRecords.AsQueryable().GetEnumerator());
			mockSet.As<IAsyncEnumerable<HoldDate>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>())).Returns(new TestAsyncEnumerator<HoldDate>(existingRecords.GetEnumerator()));

			_dbContext.Setup(x => x.Set<HoldDate>()).Returns(mockSet.Object);

			// Act
			var result = await _sut.ValidateBeforeUpdateAsync(holdDatesUpdateDto);

			// Assert
			result.ShouldNotBeNull();
			result.Count.ShouldBe(3);
			result.ShouldAllBe(x=>!x.IsValid);
			result[0].Message.ShouldBe("Begin hold date of 'May - June' is not in May");
			result[1].Message.ShouldBe("Begin hold date of 'June - July' is not in June");
			result[2].Message.ShouldBe("Begin hold date of 'July - Sep' is not in June");
		}

		[Fact]
		public async Task ValidateBeforeUpdateAsync_ShouldReturnInvalid_WhenUpdateReleaseDateIsOutsideTheMonthFromRules()
		{
			// Arrange
			var holdDatesUpdateDto = new HoldDatesUpdateDto
			{
				Updates = new List<HoldDateUpdateDto>
				{
					new HoldDateUpdateDto { Name = "May - June", BeginHold = new DateTime(2024, 5, 15), Release = new DateTime(2024, 6, 15) },
					new HoldDateUpdateDto { Name = "June - July", BeginHold = new DateTime(2024, 6, 15), Release = new DateTime(2024, 6, 15) },
					new HoldDateUpdateDto { Name = "July - Sep", BeginHold = new DateTime(2024, 6, 15), Release = new DateTime(2024, 10, 15) }
				}
			};

			var existingRecords = new List<HoldDate>
			{
				new HoldDate { Id = 1, Name = "May - June", BeginHold = new DateTime(2024, 5, 1), Release = new DateTime(2024, 7, 1) },
				new HoldDate { Id = 2, Name = "June - July", BeginHold = new DateTime(2024, 6, 1), Release = new DateTime(2024, 7, 1) },
				new HoldDate { Id = 3, Name = "July - Sep", BeginHold = new DateTime(2024, 6, 1), Release = new DateTime(2024, 9, 1) }
			};

			var mockSet = new Mock<DbSet<HoldDate>>();
			mockSet.As<IQueryable<HoldDate>>().Setup(m => m.Provider).Returns(existingRecords.AsQueryable().Provider);
			mockSet.As<IQueryable<HoldDate>>().Setup(m => m.Expression).Returns(existingRecords.AsQueryable().Expression);
			mockSet.As<IQueryable<HoldDate>>().Setup(m => m.ElementType).Returns(existingRecords.AsQueryable().ElementType);
			mockSet.As<IQueryable<HoldDate>>().Setup(m => m.GetEnumerator()).Returns(existingRecords.AsQueryable().GetEnumerator());
			mockSet.As<IAsyncEnumerable<HoldDate>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>())).Returns(new TestAsyncEnumerator<HoldDate>(existingRecords.GetEnumerator()));

			_dbContext.Setup(x => x.Set<HoldDate>()).Returns(mockSet.Object);

			// Act
			var result = await _sut.ValidateBeforeUpdateAsync(holdDatesUpdateDto);

			// Assert
			result.ShouldNotBeNull();
			result.Count.ShouldBe(3);
			result.ShouldAllBe(x => !x.IsValid);
			result[0].Message.ShouldBe("Release date of 'May - June' is not in July");
			result[1].Message.ShouldBe("Release date of 'June - July' is not in July");
			result[2].Message.ShouldBe("Release date of 'July - Sep' is not in Sep");
		}

		private class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
		{
			private readonly IEnumerator<T> _inner;

			public TestAsyncEnumerator(IEnumerator<T> inner)
			{
				_inner = inner;
			}

			public ValueTask DisposeAsync()
			{
				_inner.Dispose();
				return ValueTask.CompletedTask;
			}

			public ValueTask<bool> MoveNextAsync()
			{
				return new ValueTask<bool>(_inner.MoveNext());
			}

			public T Current => _inner.Current;
		}
	}
}
