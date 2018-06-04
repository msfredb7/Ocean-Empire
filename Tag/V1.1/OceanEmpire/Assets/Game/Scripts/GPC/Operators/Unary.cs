﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPComponents
{
	public abstract class Unary : IGPComponent
	{
		public IGPComponent child { get; private set; }

        protected Unary(IGPComponent child)
        {
            this.child = child;
        }

        public abstract GPCState Eval ();

		public abstract void Launch ();

		public abstract void Reset ();

		public abstract void Abort ();
	}
}