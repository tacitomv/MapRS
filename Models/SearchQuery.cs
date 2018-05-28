using System.Collections.Generic;

namespace Mapa.Models
{
    public class Query
    {
        public Query()
        {
            this.Page = 1;
            this.Size = 20;
        }
        public int Page { get; set; }
        public int Size { get; set; }
        public int Total { get; set; }
        public string Order { get; set; }
        public string Keywords { get; set; }

        public Query Clone()
        {
            var clone = (Query)this.MemberwiseClone();
            return clone;
        }

        protected virtual List<string> BuildUrl(IList<string> exclude = null)
        {
            //var properties = this.GetType().GetProperties();
            //var x = properties.Select(prop => $"{HttpUtility.UrlEncode(prop.Name)}={HttpUtility.UrlEncode(prop.GetValue(this, null)?.ToString())}");
            var query = new List<string>();
            query.Add($"?page={this.Page}");
            if (this.Size > 0 && (exclude != null ? !exclude.Contains("size") : true))
                query.Add($"size={this.Size}");
            if (!string.IsNullOrEmpty(this.Order) && (exclude != null ? !exclude.Contains("order") : true))
                query.Add($"order={this.Order}");

            return query;
        }

        public string GetUrl(IList<string> exclude = null)
        {
            return string.Join("&", this.BuildUrl(exclude));
        }

        public bool HasKeywords() => !string.IsNullOrEmpty(this.Keywords);
    }

    public class POIsQuery : Query
	{
		public POIsQuery() : base()
		{
			Order = "lastSeen";
		}
		public string Active { get; set; }

		protected override List<string> BuildUrl(IList<string> exclude = null)
		{
			var query = base.BuildUrl(exclude);
			if (!string.IsNullOrWhiteSpace(this.Active))
				query.Add($"tag={this.Active}");
			return query;
		}
	}
}