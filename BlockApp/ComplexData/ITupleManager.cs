using System;
using System.Collections.Generic;
using System.Text;

namespace DCRF.ComplexData
{
    /// <summary>
    /// this class acts as a type and key information source to manage type of tuples
    /// Each tuple can ask its manager what is the type of a specific key
    /// </summary>
    public interface ITupleManager
    {
        /// <summary>
        /// id can be a single ID string or of type A.B.C....D to specify parents of this ID
        /// in this case its real id is D but ABC... help manager to find the correct item
        /// </summary>
        /// <param name="id"></param>
        /// <param name="keysAndTypes"></param>
        void RegisterItemTypes(string id, params object[] keysAndTypes);

        /// <summary>
        /// Id has the form of A.B.C which denoted C child of B child of A
        /// </summary>
        /// <param name="id"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        Type GetItemType(string id, int index);

        /// <summary>
        /// Looks for item information with given id and key.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        int GetItemIndex(string id, string key);
    }
}
