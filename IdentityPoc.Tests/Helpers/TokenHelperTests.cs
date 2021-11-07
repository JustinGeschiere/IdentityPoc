using IdentityPoc.Features.Helpers;
using System;
using Xunit;

namespace IdentityPoc.Tests.Helpers
{
	public class TokenHelperTests
	{
		[Fact]
		public void TokenToGuid_ShouldBeReversedByGuidToToken()
		{
			var id = Guid.NewGuid();
			var token = TokenHelper.GuidToToken(id);

			Assert.Equal(id, TokenHelper.TokenToGuid(token));
		}

		[Fact]
		public void GuidToToken_ShouldBeReversedByTokenToGuid()
		{
			var token = TokenHelper.GuidToToken(Guid.NewGuid());
			var id = TokenHelper.TokenToGuid(token);

			Assert.Equal(token, TokenHelper.GuidToToken(id));
		}
	}
}
