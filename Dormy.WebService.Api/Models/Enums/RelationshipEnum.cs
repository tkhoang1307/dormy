using System.ComponentModel;

namespace Dormy.WebService.Api.Models.Enums
{
    public enum RelationshipEnum
    {
        [Description("Father /*-*/ Cha, Bố, Ba")]
        FATHER,

        [Description("Mother /*-*/ Mẹ, Má")]
        MOTHER,

        [Description("Older Brother /*-*/ Anh trai")]
        OLDER_BROTHER,

        [Description("Older Sister /*-*/ Chị gái")]
        OLDER_SISTER,

        [Description("Younger Brother /*-*/ Em trai")]
        YOUNGER_BROTHER,

        [Description("Younger Sister /*-*/ Em gái")]
        YOUNGER_SISTER,

        [Description("Uncle /*-*/ Chú")]
        UNCLE,

        [Description("Aunt /*-*/ Dì")]
        AUNT,

        [Description("Grandfather /*-*/ Ông")]
        GRANDFATHER,

        [Description("Grandmother /*-*/ Bà")]
        GRANDMOTHER,

        [Description("Son /*-*/ Con trai")]
        SON,

        [Description("Daughter /*-*/ Con gái")]
        DAUGHTER,

        [Description("Husband /*-*/ Chồng")]
        HUSBAND,

        [Description("Wife /*-*/ Vợ")]
        WIFE,

        [Description("Father-in-law /*-*/ Bố vợ, Bố chồng")]
        FATHER_IN_LAW,

        [Description("Mother-in-law /*-*/ Mẹ vợ, Mẹ chồng")]
        MOTHER_IN_LAW,

        [Description("Brother-in-law /*-*/ Anh rể, Em rể")]
        BROTHER_IN_LAW,

        [Description("Sister-in-law /*-*/ Chị dâu, Em dâu")]
        SISTER_IN_LAW,

        [Description("Nephew /*-*/ Cháu trai")]
        NEPHEW,

        [Description("Niece /*-*/ Cháu gái")]
        NIECE,

        [Description("Cousin /*-*/ Anh chị em họ")]
        COUSIN,

        [Description("Other /*-*/ Khác")]
        OTHER
    }
}
